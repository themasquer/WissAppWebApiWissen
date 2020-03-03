using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using SpreadsheetLight;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace AppCore.Utils.MSoffice
{
    public class ExcelUtil
    {
        public bool ExportDataToExcel(string filePath, DataSet dataSet)
        {
            try
            {
                if (dataSet == null)
                    return false;
                if (dataSet.Tables.Count <= 0)
                    return false;
                string sheetToBeDeleted = "Sheet1";
                int sheetNo = 1;
                string sheetText = "Sheet";
                string sheet;
                int startRowIndex = 1;
                int startColumnIndex = 1;
                int endRowIndex;
                int endColumnIndex;
                SLDocument slDocument = new SLDocument();
                SLStyle headerStyle = slDocument.CreateStyle();
                headerStyle.SetFontBold(true);
                headerStyle.Alignment.Horizontal = DocumentFormat.OpenXml.Spreadsheet.HorizontalAlignmentValues.Left;
                headerStyle.Fill.SetPattern(DocumentFormat.OpenXml.Spreadsheet.PatternValues.Solid, System.Drawing.Color.Khaki, System.Drawing.Color.Black);
                foreach (DataTable dataTable in dataSet.Tables)
                {
                    endColumnIndex = dataTable.Columns.Count;
                    endRowIndex = dataTable.Rows.Count + startRowIndex;
                    if (dataTable.TableName.Equals(""))
                        sheet = sheetText + sheetNo;
                    else
                        sheet = dataTable.TableName;
                    slDocument.AddWorksheet(sheet);
                    slDocument.SelectWorksheet(sheet);
                    if (sheetNo == 1)
                        slDocument.DeleteWorksheet(sheetToBeDeleted);
                    for (int i = startColumnIndex; i <= endColumnIndex; i++)
                    {
                        slDocument.SetCellStyle(startRowIndex, i, headerStyle);
                    }
                    slDocument.Filter(startRowIndex, startColumnIndex, endRowIndex, endColumnIndex);
                    slDocument.ImportDataTable(startRowIndex, startColumnIndex, dataTable, true);
                    slDocument.AutoFitColumn(startColumnIndex, endColumnIndex);
                    sheetNo++;
                }
                slDocument.SaveAs(filePath);
                return true;
            }
            catch (Exception exc)
            {
                return false;
            }
        }
        public DataTable GetDataFromSingleWorksheet(string filePath, int sheetNumber = 1)
        {
            DataTable result = new DataTable();
            int sheetIndex;
            string relationshipId;
            int rowNumber = 0;
            int columnNumber = -1;
            try
            {
                using (SpreadsheetDocument spreadSheetDocument = SpreadsheetDocument.Open(filePath, false))
                {
                    WorkbookPart workbookPart = spreadSheetDocument.WorkbookPart;
                    sheetIndex = sheetNumber - 1;
                    Sheet sheet = (Sheet)spreadSheetDocument.WorkbookPart.Workbook.Sheets.ChildElements.GetItem(sheetIndex);
                    relationshipId = sheet.Id.Value;
                    WorksheetPart worksheetPart = (WorksheetPart)spreadSheetDocument.WorkbookPart.GetPartById(relationshipId);
                    Worksheet workSheet = worksheetPart.Worksheet;
                    SheetData sheetData = workSheet.GetFirstChild<SheetData>();
                    IEnumerable<Row> rows = sheetData.Descendants<Row>();
                    foreach (Cell cell in rows.ElementAt(0))
                    {
                        result.Columns.Add(GetCellValue(spreadSheetDocument, cell));
                    }
                    foreach (Row row in rows)
                    {
                        DataRow dataRow = result.NewRow();
                        rowNumber++;
                        for (int i = 0; i < row.Descendants<Cell>().Count(); i++)
                        {
                            Cell cell = row.Descendants<Cell>().ElementAt(i);
                            int actualCellIndex = CellReferenceToIndex(cell);
                            columnNumber = actualCellIndex + 1;
                            dataRow[actualCellIndex] = GetCellValue(spreadSheetDocument, cell);
                        }
                        result.Rows.Add(dataRow);
                    }
                }
                result.Rows.RemoveAt(0);
            }
            catch (Exception exc)
            {
                result = new DataTable();
                result.Columns.Add("Error Row Number");
                result.Columns.Add("Error Column Number");
                DataRow dataRow = result.NewRow();
                dataRow[0] = rowNumber;
                dataRow[1] = columnNumber;
                result.Rows.Add(dataRow);
            }
            return result;
        }

        private string GetCellValue(SpreadsheetDocument document, Cell cell)
        {
            string cellValue;
            SharedStringTablePart stringTablePart = document.WorkbookPart.SharedStringTablePart;
            if (cell.CellValue == null)
            {
                cellValue = "";
            }
            else
            {
                string value = cell.CellValue.InnerXml;
                if (cell.DataType != null && cell.DataType.Value == CellValues.SharedString)
                {
                    cellValue = stringTablePart.SharedStringTable.ChildElements[Int32.Parse(value)].InnerText;
                }
                else
                {
                    cellValue = value;
                }
            }
            return cellValue;
        }

        private int CellReferenceToIndex(Cell cell)
        {
            int index = 0;
            string reference = cell.CellReference.ToString().ToUpper();
            foreach (char ch in reference)
            {
                if (Char.IsLetter(ch))
                {
                    int value = (int)ch - (int)'A';
                    index = (index == 0) ? value : ((index + 1) * 26) + value;
                }
                else
                    return index;
            }
            return index;
        }
    }
}
