using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace OwaspSAMM.Web
{
    /// <summary>
    /// OxmlCellFormat - enum that points to the CellFormat items in the stylesheet part of the worksheet.  The cell format items are
    /// used to set the format of a cell.
    /// </summary>
    public enum OxmlCellFormat
    {
        Default,
        Bold,
        BoldGold,
        BoldBlack,
        TextWhite,
        BoldRight,
        DefaultWrap,
        DefaultWrapRight,
        RedFill,
        RedRight,
        OrangeLeft,
        OrangeRight
    }

    public class OpenXMLExcelHelper
    {
        private SpreadsheetDocument SpreadSheet { get; set; }
        private SharedStringTablePart shareStringPart { get; set; }
        private WorksheetPart worksheetPart { get; set; }

        #region Public Worksheet Methods
        public void CreateWorkbook(MemoryStream stream, string worksheetName = "Sheet1")
        {
            SpreadSheet = SpreadsheetDocument.Create(stream, SpreadsheetDocumentType.Workbook, true);

            // Setup the new workbook.  Add WorkbookPart and Workbook
            WorkbookPart workbookPart = SpreadSheet.AddWorkbookPart();
            workbookPart.Workbook = new Workbook();

            // Add Stylesheet
            var stylesPart = workbookPart.AddNewPart<WorkbookStylesPart>();
            stylesPart.Stylesheet = new Stylesheet();
            BuildStylesheet(stylesPart);

            // End of code to add Stylesheet to workbook

            // Add a Sheet to the Workbook.
            Sheets sheets = SpreadSheet.WorkbookPart.Workbook.AppendChild<Sheets>(new Sheets());

            // This pattern seems like extra work, but the guidance from Microsoft is to add strings to the SharedStringTable and then 
            // reference the item in the cell.  This is the pattern that was followed when adding cells to the worksheet.  One could add 
            // strings directly to cells, but we chose to follow MS guidance here.

            // Get the SharedStringTablePart. If it does not exist, create a new one.
            //SharedStringTablePart shareStringPart;

            if (SpreadSheet.WorkbookPart.GetPartsOfType<SharedStringTablePart>().Count() > 0)
            {
                shareStringPart = SpreadSheet.WorkbookPart.GetPartsOfType<SharedStringTablePart>().First();
            }
            else
            {
                shareStringPart = SpreadSheet.WorkbookPart.AddNewPart<SharedStringTablePart>();
            }

            // Insert a new worksheet.
            worksheetPart = InsertWorksheet(SpreadSheet.WorkbookPart, worksheetName);
        }

        public void WriteStringToCell(string s, uint columnIndex, uint rowIndex, uint styleIndex = 0)
        {
            int ssIndex = InsertSharedStringItem(s, shareStringPart);
            Cell cell = InsertCellInWorksheet(columnIndex.ToAlphabetChar(), rowIndex, worksheetPart);
            cell.CellValue = new CellValue(ssIndex.ToString());
            cell.DataType = new EnumValue<CellValues>(CellValues.SharedString);
            cell.StyleIndex = styleIndex;

        }

        public void WriteNumberToCell(string s, uint columnIndex, uint rowIndex, uint styleIndex = 0)
        {
            Cell cell = InsertCellInWorksheet(columnIndex.ToAlphabetChar(), rowIndex, worksheetPart);
            cell.DataType = new EnumValue<CellValues>(CellValues.Number);
            cell.CellValue = new CellValue(s);
        }

        public void MergeCells(uint col1, uint row1, uint col2, uint row2)
        {
            MergeCells((uint)col1, (uint)row1, (uint)col2, (uint)row2, worksheetPart);

        }

        public void SetColumnWidth(uint col, DoubleValue size)
        {
            SetColumnWidth(worksheetPart.Worksheet, col, size);
        }

        public void Save()
        {
            worksheetPart.Worksheet.Save();
        }

        /// <summary>
        /// Closes document and writes relationship data to file.
        /// </summary>
        public void Close()
        {
            SpreadSheet.Close();
        }
        #endregion

        #region Internal Export Helper Methods

        private void BuildStylesheet(WorkbookStylesPart stylesPart)
        {
            // blank font list
            stylesPart.Stylesheet.Fonts = new Fonts();
            stylesPart.Stylesheet.Fonts.AppendChild(new Font());
            stylesPart.Stylesheet.Fonts.AppendChild(new Font(new Bold()));
            stylesPart.Stylesheet.Fonts.AppendChild(new Font(new Bold(), new Color() { Rgb = new HexBinaryValue() { Value = "FFFF0000" } })); // Red
            stylesPart.Stylesheet.Fonts.AppendChild(new Font(new Bold(), new Color() { Rgb = new HexBinaryValue() { Value = "FFFF6600" } })); // Orange
            stylesPart.Stylesheet.Fonts.AppendChild(new Font(new Bold(), new Color() { Rgb = new HexBinaryValue() { Value = "FFFFFFFF" } })); // White

            // create fills
            stylesPart.Stylesheet.Fills = new Fills();
            stylesPart.Stylesheet.Fills.AppendChild(new Fill { PatternFill = new PatternFill { PatternType = PatternValues.None } }); // required, reserved by Excel
            stylesPart.Stylesheet.Fills.AppendChild(new Fill { PatternFill = new PatternFill { PatternType = PatternValues.Gray125 } }); // required, reserved by Excel
            stylesPart.Stylesheet.Fills.AppendChild(new Fill { PatternFill = new PatternFill { PatternType = PatternValues.LightGray } });
            stylesPart.Stylesheet.Fills.AppendChild(new Fill { PatternFill = new PatternFill { PatternType = PatternValues.Solid } });
            stylesPart.Stylesheet.Fills.AppendChild(new Fill(new PatternFill(new ForegroundColor() { Rgb = new HexBinaryValue() { Value = "FFFFC000" } }) { PatternType = PatternValues.Solid }));
            stylesPart.Stylesheet.Fills.AppendChild(new Fill(new PatternFill(new ForegroundColor() { Rgb = new HexBinaryValue() { Value = "FFE6B8B7" } }) { PatternType = PatternValues.Solid }));
            stylesPart.Stylesheet.Fills.AppendChild(new Fill(new PatternFill(new ForegroundColor() { Rgb = new HexBinaryValue() { Value = "FFFCD5B4" } }) { PatternType = PatternValues.Solid }));

            //stylesPart.Stylesheet.Fills.Count = 2;

            // blank border list
            stylesPart.Stylesheet.Borders = new Borders();
            //stylesPart.Stylesheet.Borders.Count = 1;
            stylesPart.Stylesheet.Borders.AppendChild(new Border());

            // blank cell format list
            stylesPart.Stylesheet.CellStyleFormats = new CellStyleFormats();
            stylesPart.Stylesheet.CellStyleFormats.Count = 1;
            stylesPart.Stylesheet.CellStyleFormats.AppendChild(new CellFormat());

            // cell format list
            // **********************************************
            //THE ENUM AT THE TOP OF THE CLASS IS USED AS AN INDEX TO THE FOLLOWING CELLFORMATS.  If the items below are changed, 
            // ensure that the enum is updated accordingly.
            // **********************************************
            stylesPart.Stylesheet.CellFormats = new CellFormats();
            // empty one for index 0, seems to be required
            // cell format references style format 0, font 0, border 0, fill 2 and applies the fill
            stylesPart.Stylesheet.CellFormats.AppendChild(new CellFormat());                                            // index 0
            stylesPart.Stylesheet.CellFormats.AppendChild(new CellFormat { FormatId = 0, FontId = 1, FillId = 0 });     // index 1
            stylesPart.Stylesheet.CellFormats.AppendChild(new CellFormat { FormatId = 0, FontId = 1, FillId = 4 });     // index 2
            stylesPart.Stylesheet.CellFormats.AppendChild(new CellFormat { FormatId = 0, FontId = 0, FillId = 3 });     // index 3
            stylesPart.Stylesheet.CellFormats.AppendChild(new CellFormat { FormatId = 0, FontId = 3, FillId = 0 });     // index 4
            stylesPart.Stylesheet.CellFormats.AppendChild(new CellFormat { FormatId = 0, FontId = 1, FillId = 0 }).AppendChild(new Alignment { Vertical = VerticalAlignmentValues.Top, WrapText = true, Horizontal = HorizontalAlignmentValues.Right });
            stylesPart.Stylesheet.CellFormats.AppendChild(new CellFormat { FormatId = 0, FontId = 0 }).AppendChild(new Alignment { Vertical = VerticalAlignmentValues.Top, WrapText = true });
            stylesPart.Stylesheet.CellFormats.AppendChild(new CellFormat { FormatId = 0, FontId = 0 }).AppendChild(new Alignment { Vertical = VerticalAlignmentValues.Center, WrapText = true, Horizontal = HorizontalAlignmentValues.Right });
            stylesPart.Stylesheet.CellFormats.AppendChild(new CellFormat { FormatId = 0, FontId = 2, FillId = 5 }).AppendChild(new Alignment { Vertical = VerticalAlignmentValues.Center, WrapText = true });
            stylesPart.Stylesheet.CellFormats.AppendChild(new CellFormat { FormatId = 0, FontId = 2, FillId = 5 }).AppendChild(new Alignment { Vertical = VerticalAlignmentValues.Center, WrapText = true, Horizontal = HorizontalAlignmentValues.Right });
            stylesPart.Stylesheet.CellFormats.AppendChild(new CellFormat { FormatId = 0, FontId = 3, FillId = 6 }).AppendChild(new Alignment { Vertical = VerticalAlignmentValues.Center, WrapText = true });
            stylesPart.Stylesheet.CellFormats.AppendChild(new CellFormat { FormatId = 0, FontId = 3, FillId = 6 }).AppendChild(new Alignment { Vertical = VerticalAlignmentValues.Center, WrapText = true, Horizontal = HorizontalAlignmentValues.Right });
            //stylesPart.Stylesheet.CellFormats.Count = 3;
            stylesPart.Stylesheet.Save();

        }


        // Given text and a SharedStringTablePart, creates a SharedStringItem with the specified text 
        // and inserts it into the SharedStringTablePart. If the item already exists, returns its index.
        private int InsertSharedStringItem(string text, SharedStringTablePart shareStringPart)
        {
            // If the part does not contain a SharedStringTable, create one.
            if (shareStringPart.SharedStringTable == null)
            {
                shareStringPart.SharedStringTable = new SharedStringTable();
            }

            int i = 0;

            // Iterate through all the items in the SharedStringTable. If the text already exists, return its index.
            foreach (SharedStringItem item in shareStringPart.SharedStringTable.Elements<SharedStringItem>())
            {
                if (item.InnerText == text)
                {
                    return i;
                }

                i++;
            }

            // The text does not exist in the part. Create the SharedStringItem and return its index.
            shareStringPart.SharedStringTable.AppendChild(new SharedStringItem(new DocumentFormat.OpenXml.Spreadsheet.Text(text)));
            shareStringPart.SharedStringTable.Save();

            return i;
        }

        // Given a WorkbookPart, inserts a new worksheet.
        private WorksheetPart InsertWorksheet(WorkbookPart workbookPart, string worksheetName)
        {
            // Add a new worksheet part to the workbook.
            WorksheetPart newWorksheetPart = workbookPart.AddNewPart<WorksheetPart>();
            newWorksheetPart.Worksheet = new Worksheet(new SheetData());
            newWorksheetPart.Worksheet.Save();

            Sheets sheets = workbookPart.Workbook.GetFirstChild<Sheets>();
            string relationshipId = workbookPart.GetIdOfPart(newWorksheetPart);

            // Get a unique ID for the new sheet.
            uint sheetId = 1;
            if (sheets.Elements<Sheet>().Count() > 0)
            {
                sheetId = sheets.Elements<Sheet>().Select(s => s.SheetId.Value).Max() + 1;
            }

            //string sheetName = "Sheet" + sheetId;

            // Append the new worksheet and associate it with the workbook.
            Sheet sheet = new Sheet() { Id = relationshipId, SheetId = sheetId, Name = worksheetName };
            sheets.Append(sheet);
            workbookPart.Workbook.Save();

            return newWorksheetPart;
        }

        // Given a column name, a row index, and a WorksheetPart, inserts a cell into the worksheet. 
        // If the cell already exists, returns it. 
        private Cell InsertCellInWorksheet(string columnName, uint rowIndex, WorksheetPart worksheetPart)
        {
            Worksheet worksheet = worksheetPart.Worksheet;
            SheetData sheetData = worksheet.GetFirstChild<SheetData>();
            string cellReference = columnName + rowIndex;

            // If the worksheet does not contain a row with the specified row index, insert one.
            Row row;
            if (sheetData.Elements<Row>().Where(r => r.RowIndex == rowIndex).Count() != 0)
            {
                row = sheetData.Elements<Row>().Where(r => r.RowIndex == rowIndex).First();
            }
            else
            {
                row = new Row() { RowIndex = rowIndex };
                sheetData.Append(row);
            }

            // If there is not a cell with the specified column name, insert one.  
            if (row.Elements<Cell>().Where(c => c.CellReference.Value == columnName + rowIndex).Count() > 0)
            {
                return row.Elements<Cell>().Where(c => c.CellReference.Value == cellReference).First();
            }
            else
            {
                // Cells must be in sequential order according to CellReference. Determine where to insert the new cell.
                Cell refCell = null;
                foreach (Cell cell in row.Elements<Cell>())
                {
                    if (string.Compare(cell.CellReference.Value, cellReference, true) > 0)
                    {
                        refCell = cell;
                        break;
                    }
                }

                Cell newCell = new Cell() { CellReference = cellReference };
                row.InsertBefore(newCell, refCell);

                worksheet.Save();
                return newCell;
            }
        }

        private void MergeCells(uint col1, uint row1, uint col2, uint row2, WorksheetPart worksheetPart)
        {
            // Create the cells in case they don't already exist
            for (uint i = col1; i < col2; i++)
            {
                for (uint j = row1; j < row2; j++)
                {
                    InsertCellInWorksheet(i.ToAlphabetChar(), j, worksheetPart);
                }
            }

            MergeCells mergeCells;
            Worksheet worksheet = worksheetPart.Worksheet;

            if (worksheet.Elements<MergeCells>().Count() > 0)
            {
                mergeCells = worksheet.Elements<MergeCells>().First();
            }
            else
            {
                mergeCells = new MergeCells();

                // Insert a MergeCells object into the specified position.
                if (worksheet.Elements<CustomSheetView>().Count() > 0)
                {
                    worksheet.InsertAfter(mergeCells, worksheet.Elements<CustomSheetView>().First());
                }
                else if (worksheet.Elements<DataConsolidate>().Count() > 0)
                {
                    worksheet.InsertAfter(mergeCells, worksheet.Elements<DataConsolidate>().First());
                }
                else if (worksheet.Elements<SortState>().Count() > 0)
                {
                    worksheet.InsertAfter(mergeCells, worksheet.Elements<SortState>().First());
                }
                else if (worksheet.Elements<AutoFilter>().Count() > 0)
                {
                    worksheet.InsertAfter(mergeCells, worksheet.Elements<AutoFilter>().First());
                }
                else if (worksheet.Elements<Scenarios>().Count() > 0)
                {
                    worksheet.InsertAfter(mergeCells, worksheet.Elements<Scenarios>().First());
                }
                else if (worksheet.Elements<ProtectedRanges>().Count() > 0)
                {
                    worksheet.InsertAfter(mergeCells, worksheet.Elements<ProtectedRanges>().First());
                }
                else if (worksheet.Elements<SheetProtection>().Count() > 0)
                {
                    worksheet.InsertAfter(mergeCells, worksheet.Elements<SheetProtection>().First());
                }
                else if (worksheet.Elements<SheetCalculationProperties>().Count() > 0)
                {
                    worksheet.InsertAfter(mergeCells, worksheet.Elements<SheetCalculationProperties>().First());
                }
                else
                {
                    worksheet.InsertAfter(mergeCells, worksheet.Elements<SheetData>().First());
                }
            }

            string cell1Name = col1.ToAlphabetChar() + row1;
            string cell2Name = col2.ToAlphabetChar() + row2;

            // Create the merged cell and append it to the MergeCells collection.
            MergeCell mergeCell = new MergeCell() { Reference = new StringValue(cell1Name + ":" + cell2Name) };
            mergeCells.Append(mergeCell);

            worksheet.Save();
        }

        private void SetColumnWidth(Worksheet worksheet, uint Index, DoubleValue dwidth)
        {
            DocumentFormat.OpenXml.Spreadsheet.Columns cs = worksheet.GetFirstChild<DocumentFormat.OpenXml.Spreadsheet.Columns>();
            if (cs != null)
            {
                IEnumerable<DocumentFormat.OpenXml.Spreadsheet.Column> ic = cs.Elements<DocumentFormat.OpenXml.Spreadsheet.Column>().Where(r => r.Min == Index).Where(r => r.Max == Index);
                if (ic.Count() > 0)
                {
                    DocumentFormat.OpenXml.Spreadsheet.Column c = ic.First();
                    c.Width = dwidth;
                }
                else
                {
                    DocumentFormat.OpenXml.Spreadsheet.Column c = new DocumentFormat.OpenXml.Spreadsheet.Column() { Min = Index, Max = Index, Width = dwidth, CustomWidth = true };
                    cs.Append(c);
                }
            }
            else
            {
                cs = new DocumentFormat.OpenXml.Spreadsheet.Columns();
                DocumentFormat.OpenXml.Spreadsheet.Column c = new DocumentFormat.OpenXml.Spreadsheet.Column() { Min = Index, Max = Index, Width = dwidth, CustomWidth = true };
                cs.Append(c);
                worksheet.InsertAfter(cs, worksheet.GetFirstChild<SheetFormatProperties>());
            }
        }
        #endregion
    }


    public static class OpenXMLExtensions
    {
        /// <summary>
        /// Build the Column name of the Cell address.  The first 26 columns are A-Z.  The next are AA-AZ, BA-BZ, CA-CZ, etc.
        /// This method takes an unsigned int and converts it to the right letter(s)
        /// </summary>
        /// <param name="uint x - column index"></param>
        /// <returns>Character prepresenting the column x</returns>
        public static string ToAlphabetChar(this uint x)
        {
            // Modulus division is used to map the column integer back to a character.
            // Z is the first char because of the zero based array.  The last letter Z will be 26 mod 26 => 0
            string alphabet = "ZABCDEFGHIJKLMNOPQRSTUVWXY";

            int quotient = (int)x / 26;
            int remainder = (int)x % 26;

            string result = quotient > 0 ? alphabet[quotient].ToString() : string.Empty;
            result += alphabet[remainder].ToString();
            return result;
        }
    }

}