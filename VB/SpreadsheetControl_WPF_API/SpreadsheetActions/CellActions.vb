﻿Imports System
Imports System.Drawing
Imports DevExpress.Spreadsheet

Namespace SpreadsheetControl_WPF_API
    Public NotInheritable Class CellActions

        Private Sub New()
        End Sub

#Region "Actions"
        Public Shared CreateSimpleAndComplexRangesAction As Action(Of IWorkbook) = AddressOf CreateSimpleAndComplexRanges
        Public Shared ChangeCellValueAction As Action(Of IWorkbook) = AddressOf ChangeCellValue
        Public Shared CellValueToFromObjectAction As Action(Of IWorkbook) = AddressOf CellValueToFromObject
        Public Shared CellValueFromObjectViaCustomConverterAction As Action(Of IWorkbook) = AddressOf CellValueFromObjectViaCustomConverter
        Public Shared AddHyperlinkAction As Action(Of IWorkbook) = AddressOf AddHyperlink
        Public Shared AddCommentAction As Action(Of IWorkbook) = AddressOf AddComment
        Public Shared CopyCellDataAndStyleAction As Action(Of IWorkbook) = AddressOf CopyCellDataAndStyle
        Public Shared MergeAndSplitCellsAction As Action(Of IWorkbook) = AddressOf MergeAndSplitCells
        Public Shared ClearCellsAction As Action(Of IWorkbook) = AddressOf ClearCells
#End Region
        Private Shared Sub CreateSimpleAndComplexRanges(ByVal workbook As IWorkbook)
            workbook.BeginUpdate()
            Try
                Dim worksheet As Worksheet = workbook.Worksheets(0)
                Dim myColor1 As Color = workbook.Styles("20% - Accent1").Fill.BackgroundColor
                Dim myColor2 As Color = workbook.Styles("20% - Accent2").Fill.BackgroundColor

                '				#Region "#SimpleRange"
                ' A range that includes cells from the top left cell (A1) to the bottom right cell (B5).
                Dim rangeA1B5 As CellRange = worksheet("A1:B5")

                ' A rectangular range that includes cells from the top left cell (C5) to the bottom right cell (E7).
                Dim rangeC5E7 As CellRange = worksheet("C5:E7")

                ' The C4:E7 cell range located in the "Sheet3" worksheet.
                Dim rangeSheet3C4E7 As CellRange = workbook.Range("Sheet3!C4:E7")

                ' A range that contains a single cell (E7).
                Dim rangeE7 As CellRange = worksheet("E7")

                ' A range that includes the entire column A.
                Dim rangeColumnA As CellRange = worksheet("A:A")

                ' A range that includes the entire row 5.
                Dim rangeRow5 As CellRange = worksheet("5:5")

                ' A minimal rectangular range that includes all listed cells: C6, D9 and E7.
                Dim rangeC6D9E7 As CellRange = worksheet.Range.Parse("C6:D9:E7")

                ' A rectangular range whose left column index is 0, top row index is 0, 
                ' right column index is 3 and bottom row index is 2. This is the A1:D3 cell range.
                Dim rangeA1D3 As CellRange = worksheet.Range.FromLTRB(0, 0, 3, 2)

                ' A range that includes the intersection of two ranges: C5:E10 and E9:G13. 
                ' This is the E9:E10 cell range.
                Dim rangeE9E10 As CellRange = worksheet("C5:E10 E9:G13")

                ' Create a defined name for the D20:G23 cell range.
                worksheet.DefinedNames.Add("MyNamedRange", "Sheet1!$D$20:$G$23")
                ' Access a range by its defined name.
                Dim rangeD20G23 As CellRange = worksheet("MyNamedRange")
                '				#End Region ' #SimpleRange

                '				#Region "#ComplexRange"
                Dim rangeA1D4 As CellRange = worksheet("A1:D4")
                Dim rangeD5E7 As CellRange = worksheet("D5:E7")
                Dim rangeRow11 As CellRange = worksheet("11:11")
                Dim rangeF7 As CellRange = worksheet("F7")

                ' Create a complex range using the Range.Union method.
                Dim complexRange1 As CellRange = worksheet("A7:A9").Union(rangeD5E7)

                ' Create a complex range using the IRangeProvider.Union method.
                Dim complexRange2 As CellRange = worksheet.Range.Union(New CellRange() {rangeRow11, rangeA1D4, rangeF7})

                ' Fill the ranges with different colors.
                complexRange1.FillColor = myColor1
                complexRange2.FillColor = myColor2

                ' Use the Areas property to get access to a component of a complex range.
                complexRange2.Areas(2).FillColor = Color.Beige
                '				#End Region ' #ComplexRange
            Finally
                workbook.EndUpdate()
            End Try

        End Sub
        Private Shared Sub ChangeCellValue(ByVal workbook As IWorkbook)
            workbook.BeginUpdate()
            Try
                Dim worksheet As Worksheet = workbook.Worksheets(0)

                worksheet.Cells("A1").Value = "dateTime:"
                worksheet.Cells("A2").Value = "double:"
                worksheet.Cells("A3").Value = "string:"
                worksheet.Cells("A4").Value = "error constant:"
                worksheet.Cells("A5").Value = "boolean:"
                worksheet.Cells("A6").Value = "float:"
                worksheet.Cells("A7").Value = "char:"
                worksheet.Cells("A8").Value = "int32:"
                worksheet.Cells("A10").Value = "Fill a range of cells:"
                worksheet.Columns("A").WidthInCharacters = 20
                worksheet.Columns("B").WidthInCharacters = 20
                worksheet.Range("A1:B8").Alignment.Horizontal = SpreadsheetHorizontalAlignment.Left

'                #Region "#CellValue"
                ' Add data of different types to cells.
                worksheet.Cells("B1").Value = Date.Now
                worksheet.Cells("B2").Value = Math.PI
                worksheet.Cells("B3").Value = "Have a nice day!"
                worksheet.Cells("B4").Value = CellValue.ErrorReference
                worksheet.Cells("B5").Value = True
                worksheet.Cells("B6").Value = Single.MaxValue
                worksheet.Cells("B7").Value = "a"c
                worksheet.Cells("B8").Value = Int32.MaxValue

                ' Fill all cells in the range with 10.
                worksheet.Range("B10:E10").Value = 10
'                #End Region ' #CellValue
            Finally
                workbook.EndUpdate()
            End Try

        End Sub

        Private Shared Sub CellValueToFromObject(ByVal workbook As IWorkbook)
            workbook.BeginUpdate()
            Try
                Dim worksheet As Worksheet = workbook.Worksheets(0)

                worksheet("A1").Value = "Cell values converted to objects:"
                worksheet("A5").Value = "Cell values converted from objects:"
                worksheet.Range("A1").ColumnWidthInCharacters= 31
                worksheet.Range("B1:D5").ColumnWidthInCharacters = 12

                '                #Region "#CellValueToFromObject"
                ' Add data of different types to cells of the range.
                Dim sourceRange As CellRange = worksheet("B1:B3")
                sourceRange(0).Value = "Text"
                sourceRange(1).Formula = "=PI()"
                sourceRange(2).Value = Date.Now
                sourceRange(2).NumberFormat = "d-mmm-yy"

                ' Get the number of cells in the range.
                Dim cellCount As Integer = sourceRange.RowCount * sourceRange.ColumnCount

                ' Declare an array to store elements of different types.
                Dim array(cellCount - 1) As Object

                ' Convert cell values to objects and add them to the array.
                For i As Integer = 0 To cellCount - 1
                    array(i) = sourceRange(i).Value.ToObject()
                Next i

                ' Convert array elements to cell values and assign them to cells in the fifth row. 
                For i As Integer = 0 To array.Length - 1
                    worksheet.Rows("5")(i + 1).SetValue(array(i))
                    ' An alternative way to do this is to use the CellValue.FromObject method.
                    ' worksheet.Rows["5"][i+1].Value = CellValue.FromObject(array[i]);
                Next i
'                #End Region ' #CellValueToFromObject
            Finally
                workbook.EndUpdate()
            End Try
        End Sub

        Private Shared Sub CellValueFromObjectViaCustomConverter(ByVal workbook As IWorkbook)
            workbook.BeginUpdate()
            Try
'                #Region "#CustomCellValueConverter"
                Dim worksheet As Worksheet = workbook.Worksheets(0)
                Dim cell As Cell = worksheet.Cells("A1")
                cell.FillColor = Color.Orange
                cell.Value = CellValue.FromObject(cell.FillColor, New ColorToNameConverter())
                ' ...
'                #End Region ' #CustomCellValueConverter
            Finally
                workbook.EndUpdate()
            End Try
        End Sub

        #Region "#CustomCellValueConverter"
        Private Class ColorToNameConverter
            Implements ICellValueConverter

            Private Function ICellValueConverter_ConvertToObject(ByVal value As CellValue) As Object Implements ICellValueConverter.ConvertToObject
                Return Nothing
            End Function
            Private Function ICellValueConverter_TryConvertFromObject(ByVal value As Object) As CellValue Implements ICellValueConverter.TryConvertFromObject
                Dim isColor As Boolean = value.GetType() Is GetType(Color)
                If Not isColor Then
                    Return Nothing
                End If
                Return DirectCast(value, Color).Name
            End Function
        End Class
        #End Region ' #CustomCellValueConverter

        Private Shared Sub AddHyperlink(ByVal workbook As IWorkbook)
            workbook.BeginUpdate()
            Try
                Dim worksheet As Worksheet = workbook.Worksheets(0)
                worksheet.Range("A:C").ColumnWidthInCharacters = 12

'                #Region "#AddHyperlink"
                ' Create a hyperlink to a web page.
                Dim cell As Cell = worksheet.Cells("A1")
                worksheet.Hyperlinks.Add(cell, "http://www.devexpress.com/", True, "DevExpress")

                ' Create a hyperlink to a cell range in a workbook.
                Dim range As CellRange = worksheet.Range("C3:D4")
                Dim cellHyperlink As Hyperlink = worksheet.Hyperlinks.Add(range, "Sheet2!B2:E7", False, "Select Range")
                cellHyperlink.TooltipText = "Click Me"
'                #End Region ' #AddHyperlink
            Finally
                workbook.EndUpdate()
            End Try
        End Sub

        Private Shared Sub CopyCellDataAndStyle(ByVal workbook As IWorkbook)
            workbook.BeginUpdate()
            Try
'                #Region "#CopyCell"
                Dim worksheet As Worksheet = workbook.Worksheets(0)
                worksheet.Columns("A").WidthInCharacters = 32
                worksheet.Columns("B").WidthInCharacters = 20
                Dim style As Style = workbook.Styles(BuiltInStyleId.Input)

                ' Specify the content and formatting for a source cell.
                worksheet.Cells("A1").Value = "Source Cell"

                Dim sourceCell As Cell = worksheet.Cells("B1")
                sourceCell.Formula = "= PI()"
                sourceCell.NumberFormat = "0.0000"
                sourceCell.Style = style
                sourceCell.Font.Color = Color.Blue
                sourceCell.Font.Bold = True
                sourceCell.Borders.SetOutsideBorders(Color.Black, BorderLineStyle.Thin)

                ' Copy all information from the source cell to the "B3" cell. 
                worksheet.Cells("A3").Value = "Copy All"
                worksheet.Cells("B3").CopyFrom(sourceCell)

                ' Copy only the source cell content (e.g., text, numbers, formula calculated values) to the "B4" cell.
                worksheet.Cells("A4").Value = "Copy Values"
                worksheet.Cells("B4").CopyFrom(sourceCell, PasteSpecial.Values)

                ' Copy the source cell content (e.g., text, numbers, formula calculated values) 
                ' and number formats to the "B5" cell.
                worksheet.Cells("A5").Value = "Copy Values and Number Formats"
                worksheet.Cells("B5").CopyFrom(sourceCell, PasteSpecial.Values Or PasteSpecial.NumberFormats)

                ' Copy only the formatting information from the source cell to the "B6" cell.
                worksheet.Cells("A6").Value = "Copy Formats"
                worksheet.Cells("B6").CopyFrom(sourceCell, PasteSpecial.Formats)

                ' Copy all information from the source cell to the "B7" cell except for border settings.
                worksheet.Cells("A7").Value = "Copy All Except Borders"
                worksheet.Cells("B7").CopyFrom(sourceCell, PasteSpecial.All And (Not PasteSpecial.Borders))

                ' Copy information only about borders from the source cell to the "B8" cell.
                worksheet.Cells("A8").Value = "Copy Borders"
                worksheet.Cells("B8").CopyFrom(sourceCell, PasteSpecial.Borders)
'                #End Region ' #CopyCell
            Finally
                workbook.EndUpdate()
            End Try
        End Sub

        Private Shared Sub MergeAndSplitCells(ByVal workbook As IWorkbook)
            workbook.BeginUpdate()
            Try
                Dim worksheet As Worksheet = workbook.Worksheets(0)

                worksheet.Cells("A2").FillColor = Color.LightGray

                worksheet.Cells("B2").Value = "B2"
                worksheet.Cells("B2").FillColor = Color.LightGreen

                worksheet.Cells("C3").Value = "C3"
                worksheet.Cells("C3").FillColor = Color.LightSalmon

'                #Region "#MergeCells"
                ' Merge cells contained in the range.
                worksheet.MergeCells(worksheet.Range("A1:C5"))
'                #End Region ' #MergeCells
            Finally
                workbook.EndUpdate()
            End Try
        End Sub

        Private Shared Sub ClearCells(ByVal workbook As IWorkbook)
            workbook.BeginUpdate()
            Try
                Dim worksheet As Worksheet = workbook.Worksheets(0)

                worksheet.Range("A:D").ColumnWidthInCharacters = 30
                worksheet.Range("B1:D6").Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center

                worksheet("B1").Value = "Initial Cell Content and Formatting:"
                worksheet.MergeCells(worksheet("C1:D1"))
                worksheet("C1:D1").Value = "Cleared Cells:"

                worksheet("A2").Value = "Clear All:"
                worksheet("A3").Value = "Clear Cell Content Only:"
                worksheet("A4").Value = "Clear Cell Formatting Only:"
                worksheet("A5").Value = "Clear Cell Hyperlinks Only:"
                worksheet("A6").Value = "Clear Cell Comments Only:"

                ' Specify initial content and formatting for cells.
                Dim sourceCells As CellRange = worksheet("B2:D6")
                sourceCells.Value = Date.Now
                sourceCells.Style = workbook.Styles(BuiltInStyleId.Accent3_40percent)
                sourceCells.Font.Color = Color.LightSeaGreen
                sourceCells.Font.Bold = True
                sourceCells.Borders.SetAllBorders(Color.Blue, BorderLineStyle.Dashed)
                worksheet.Hyperlinks.Add(worksheet("B5"), "http://www.devexpress.com/", True, "DevExpress")
                worksheet.Hyperlinks.Add(worksheet("C5"), "http://www.devexpress.com/", True, "DevExpress")
                worksheet.Hyperlinks.Add(worksheet("D5"), "http://www.devexpress.com/", True, "DevExpress")
                worksheet.Comments.Add(worksheet("B6"), "Author", "Cell Comment")
                worksheet.Comments.Add(worksheet("C6"), "Author", "Cell Comment")
                worksheet.Comments.Add(worksheet("D6"), "Author", "Cell Comment")


'                #Region "#ClearCell"
                ' Remove all cell information (content, formatting, hyperlinks and comments).
                worksheet.Clear(worksheet("C2:D2"))

                ' Remove cell content.
                worksheet.ClearContents(worksheet("C3"))
                worksheet("D3").Value = Nothing

                ' Remove cell formatting.
                worksheet.ClearFormats(worksheet("C4"))
                worksheet("D4").Style = workbook.Styles.DefaultStyle

                ' Remove hyperlinks from cells.
                worksheet.ClearHyperlinks(worksheet("C5"))

                Dim hyperlinkD5 As Hyperlink = worksheet.Hyperlinks.GetHyperlinks(worksheet("D5"))(0)
                worksheet.Hyperlinks.Remove(hyperlinkD5)

                ' Remove comments from cells.
                worksheet.ClearComments(worksheet("C6"))

                Dim commentD6 As Comment = worksheet.Comments.GetComments(worksheet("D6"))(0)
                worksheet.Comments.Remove(commentD6)
'                #End Region ' #ClearCell
            Finally
                workbook.EndUpdate()
            End Try
        End Sub
        Private Shared Sub AddComment(ByVal workbook As IWorkbook)
            workbook.BeginUpdate()
            Try
                Dim worksheet As Worksheet = workbook.Worksheets(0)

                ' Specify initial content and formatting for cells.
                worksheet.Cells("A2").Value = "Original comment"
                worksheet.Cells("E2").Value = "Copied comment"
                worksheet("A2").Alignment.WrapText = True
                worksheet("E2").Alignment.WrapText = True

'                #Region "#AddComment"
                ' Get the system username. 
                Dim author As String = workbook.CurrentAuthor

                ' Add a comment to the "A2" cell.
                Dim commentedCell As Cell = worksheet.Cells("A2")
                Dim commentA2 As Comment = worksheet.Comments.Add(commentedCell, author, "This is a comment")
                commentA2.Visible = True

                ' Insert the author's name at the beginning of the comment.
                Dim commentRunsA2 As CommentRunCollection = commentA2.Runs
                commentRunsA2.Insert(0, author & ": " & ControlChars.CrLf)

                ' Copy the comment to the "E2" cell.
                worksheet.Cells("E2").CopyFrom(commentedCell, PasteSpecial.Comments)

                ' Get the added comment and make it visible.
                Dim commentE2 As Comment = worksheet.Comments.GetComments(worksheet("E2"))(0)
                commentE2.Visible = True

                ' Modify text of the copied comment.
                Dim commentRunsE2 As CommentRunCollection = commentE2.Runs
                commentRunsE2(1).Text = "This comment is copied from the cell " & commentedCell.GetReferenceA1()
'                #End Region ' #AddComment
            Finally
                workbook.EndUpdate()
            End Try
        End Sub
    End Class
End Namespace
