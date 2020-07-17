using iTextSharp.text;
using iTextSharp.text.pdf;
using Kodasat.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Kodasat.Report
{
    public class DepositedReport
    {
        
        #region Declaration

        int _totalColumn = 2;
        Document _document;
        Font _fontstyle;
        PdfPTable _pdfTable = new PdfPTable(2);

        PdfPCell _pdfCell;
        MemoryStream _memoryStream = new MemoryStream();
        List<Deposited> _depositeds = new List<Deposited>();


        #endregion

        public byte[] PrepareReport(List<Deposited> depositeds, String DateKodas, String TimeKodas)
        {
            _depositeds = depositeds;
            #region
            _document = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _document.SetPageSize(PageSize.A4);
            _document.SetMargins(20f, 20f, 20f, 20f);
            _pdfTable.WidthPercentage = 100;
            _pdfTable.HorizontalAlignment = Element.ALIGN_LEFT;
            _fontstyle = FontFactory.GetFont("Tahoma", 8f, 1);
            PdfWriter.GetInstance(_document, _memoryStream);
            _document.Open();
            _pdfTable.SetWidths(new float[] {120f, 20f });
            #endregion
            this.ReportHeader(DateKodas,TimeKodas);
            this.ReportBody();
            _pdfTable.HeaderRows = 2;
            _document.Add(_pdfTable);
            _document.Close();
            return _memoryStream.ToArray();

        }
        private void ReportHeader(String DateKodas,String TimeKodas)
        {
            _fontstyle = FontFactory.GetFont("Tahoma", 14f, 1);
            _pdfCell = new PdfPCell(new Phrase(DateKodas, _fontstyle));
            _pdfCell.Colspan = _totalColumn;
            _pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfCell.Border = 0;
            _pdfCell.BackgroundColor = BaseColor.WHITE;
            _pdfCell.ExtraParagraphSpace = 0;
            _pdfTable.AddCell(_pdfCell);
            _pdfTable.CompleteRow();

            _fontstyle = FontFactory.GetFont("Tahoma", 14f, 1);
            _pdfCell = new PdfPCell(new Phrase(TimeKodas, _fontstyle));
            _pdfCell.Colspan = _totalColumn;
            _pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfCell.Border = 0;
            _pdfCell.BackgroundColor = BaseColor.WHITE;
            _pdfCell.ExtraParagraphSpace = 0;
            _pdfTable.AddCell(_pdfCell);
            _pdfTable.CompleteRow();
        }

        private void ReportBody()
        {
            #region Table header
            _fontstyle = FontFactory.GetFont("Tahoma", 13f, 1);

            _pdfCell = new PdfPCell(new Phrase("Name", _fontstyle));
            _pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _pdfCell.BackgroundColor = BaseColor.LIGHT_GRAY;
            _pdfTable.AddCell(_pdfCell);

            _pdfCell = new PdfPCell(new Phrase("NO.", _fontstyle));
            _pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _pdfCell.BackgroundColor = BaseColor.LIGHT_GRAY;
            _pdfTable.AddCell(_pdfCell);
            _pdfTable.CompleteRow();
            #endregion

            #region Table Body
            // _fontstyle = FontFactory.GetFont("Arial", 8f, 0);
            String path = HttpContext.Current.Server.MapPath("~/fonts/trado.TTF");
            BaseFont bf = BaseFont.CreateFont(path, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);

            //Set Font and Font Color
            Font font = new Font(bf, 12, Font.BOLD);
            /***********************/
            int serialNumber = 1;
            foreach (Deposited deposited in _depositeds)
            {
                _pdfCell = new PdfPCell(new Phrase(deposited.fullName.ToString() + " ", font));
                _pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _pdfCell.BackgroundColor = BaseColor.WHITE;
                _pdfCell.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                _pdfTable.AddCell(_pdfCell);

                _pdfCell = new PdfPCell(new Phrase(serialNumber++.ToString(), font));
                _pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _pdfCell.BackgroundColor = BaseColor.WHITE;
                _pdfTable.AddCell(_pdfCell);

                _pdfTable.CompleteRow();
            }
            #endregion
        }

    }
}