using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Word = Microsoft.Office.Interop.Word;

namespace Student.DocumentGenerator
{
    public static class DocumentWordGenerator
    {
        /// <summary>
        /// Сохранить как... (диалог + Word замена)
        /// </summary>
        public static void SaveAs(string templatePath,
            string postEmployee, string employeeFullname, string desc,
            string date, List<string> students)
        {
            var saveDialog = new SaveFileDialog()
            {
                Filter = "Word (*.docx)|*.docx",
                FileName = $"Документ_{employeeFullname}.docx"
            };

            if (saveDialog.ShowDialog() == true)
            {
                GenerateDocument(templatePath, saveDialog.FileName, postEmployee, employeeFullname, desc, date, students);
                MessageBox.Show($"Сохранено: {saveDialog.FileName}", "Готово!");
            }
        }

        /// <summary>
        /// Печать (открывает Word готовый к печати)
        /// </summary>
        public static void Print(string templatePath,
            string postEmployee, string employeeFullname, string desc,
            string date, List<string> students)
        {
            string tempPath = Path.Combine(Path.GetTempPath(), $"doc_{Guid.NewGuid():N}.docx");

            try
            {
                GenerateDocument(templatePath, tempPath, postEmployee, employeeFullname, desc, date, students);
                Process.Start(new ProcessStartInfo(tempPath) { UseShellExecute = true });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}\nУстановите Microsoft Word", "Ошибка");
            }
        }

        /// <summary>
        /// Word Interop - замена плейсхолдеров
        /// </summary>
        private static void GenerateDocument(string templatePath, string outputPath,
            string postEmployee, string employeeFullname, string desc,
            string date, List<string> students)
        {
            Word.Application wordApp = null;
            Word.Document doc = null;

            try
            {
                wordApp = new Word.Application();
                doc = wordApp.Documents.Open(templatePath);

                // Обычные плейсхолдеры через Find & Replace
                ReplaceText(doc, "{post_employee}", postEmployee);
                ReplaceText(doc, "{employee_fullname}", employeeFullname);
                ReplaceText(doc, "{desc}", desc);
                ReplaceText(doc, "{date}", date);

                if (students?.Any() == true)
                {
                    // 1. Находим {student}
                    Word.Range studentRange = doc.Content;
                    studentRange.Find.ClearFormatting();
                    studentRange.Find.Text = "{student}";

                    if (studentRange.Find.Execute())
                    {
                        // 2. Заменяем {student} на первого студента
                        studentRange.Text = $" {students[0]}";

                        // 3. Делаем красную строку для первого
                        studentRange.ParagraphFormat.FirstLineIndent = -1; // 1 см

                        // 4. Ставим курсор в конец абзаца с первым студентом
                        object collapseEnd = Word.WdCollapseDirection.wdCollapseEnd;
                        studentRange.Collapse(ref collapseEnd);
                        studentRange.InsertParagraphAfter(); // новый абзац
                        studentRange.MoveStart(Word.WdUnits.wdParagraph, 1); // сдвинуться в следующий абзац
                        studentRange.Collapse(ref collapseEnd);

                        // 5. Добавляем остальных студентов, каждый в своём абзаце
                        for (int i = 1; i < students.Count; i++)
                        {
                            studentRange.Text = $" {students[i]}";
                            studentRange.ParagraphFormat.FirstLineIndent = -1;

                            // если не последний — создаём следующий абзац
                            if (i < students.Count - 1)
                            {
                                studentRange.InsertParagraphAfter();
                                studentRange.MoveStart(Word.WdUnits.wdParagraph, 1);
                                studentRange.Collapse(ref collapseEnd);
                            }
                        }
                    }
                }

                doc.SaveAs2(outputPath);
            }
            finally
            {
                doc?.Close(false);
                wordApp?.Quit();
                if (wordApp != null)
                    Marshal.ReleaseComObject(wordApp);
            }
        }

        /// <summary>
        /// Word Find & Replace (работает идеально!)
        /// </summary>
        private static void ReplaceText(Word.Document doc, string findText, string replaceText)
        {
            doc.Content.Find.ClearFormatting();
            doc.Content.Find.Replacement.ClearFormatting();

            doc.Content.Find.Execute(
                FindText: findText,
                ReplaceWith: replaceText ?? "",
                Replace: Word.WdReplace.wdReplaceAll
            );
        }
    }
}
