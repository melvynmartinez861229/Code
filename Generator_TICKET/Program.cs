using System;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Text;

namespace Generator_TICKET
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Generate PDF: ");
            
            try
            {
                string clientInfo = "100524 - MARIA GUADALUPE AGUILARA MARIN";

                FacturaCompraPDF factura = new FacturaCompraPDF();
                factura.CrearFacturaCompraPDF(56842,clientInfo);
            }
            catch (Exception e)
            {

                Console.Write("Fail");
            }
            

            Console.Write("Succefully");
            Console.ReadKey();
        }
    }

    public class FacturaCompraPDF
    {
        public void CrearFacturaCompraPDF(int ticketID,string clientInfo)
        {
            // Crear documento:
            Document doc = new Document(new Rectangle(223.94f, 800), 10, 10, 10, 10);
            PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream($"Ticket_{ticketID}.pdf", FileMode.Create));
            doc.Open();

            // Image Logo
            string rutaImagenEncabezado = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "assets", "elPunto.png");
            Image imagenEncabezado = Image.GetInstance(rutaImagenEncabezado);
            imagenEncabezado.Alignment = Element.ALIGN_CENTER;
            doc.Add(imagenEncabezado);

            // HEADER
            Paragraph pHeader = new Paragraph();
            pHeader.Leading = 9;
            Chunk chunkHeaderTitle = new Chunk("DISTRIBUIDORA FARMACEUTICA\nPERU #100 ESQ. 5 DE MAYO, TOLUCA MEX.\nC.P. 50090 SUC.TOLUCA\nCEL / WHATSAPP 722 5850951 / 722 3985672");
            chunkHeaderTitle.Font = new Font(Font.FontFamily.HELVETICA, 8.5f, Font.BOLD);
            pHeader.Add(chunkHeaderTitle);

            var font_Hight = new Font(Font.FontFamily.HELVETICA, 10, Font.BOLD);
            // TicketID
            Paragraph pTicketID = new Paragraph();
            pTicketID.SpacingBefore = 9;
            pTicketID.SpacingAfter = 9;
            pTicketID.Leading = 9;
            pTicketID.Alignment = Element.ALIGN_CENTER;
            Chunk TicketID_ = new Chunk($"TICKET: {ticketID}");
            chunkHeaderTitle.Font = font_Hight;
            pTicketID.Add(TicketID_);

            // ClientInfo
            Paragraph pClientInfo = new Paragraph();
            pClientInfo.SpacingBefore = 0;
            pClientInfo.SpacingAfter = 5;
            pClientInfo.Leading = 8;
            pClientInfo.Alignment = Element.ALIGN_LEFT;
            Chunk pClientInfo_ = new Chunk($"CLIENTE: {clientInfo}");
            pClientInfo_.Font = new Font(Font.FontFamily.HELVETICA, 8.5f, Font.BOLD);
            pClientInfo.Add(pClientInfo_);

           

            //TABLA Add Data Time
            PdfPTable tablaDataTime = new PdfPTable(2);
            tablaDataTime.DefaultCell.Padding = 0;
            tablaDataTime.DefaultCell.Border = 0;
            float[] anchosColumnas = new float[] { 2f, 1f };
            tablaDataTime.SetWidths(anchosColumnas);
            tablaDataTime.WidthPercentage = 100f;
           
            Paragraph DtaTime = new Paragraph();
            DtaTime.Leading = 8;
            DtaTime.Alignment = Element.ALIGN_LEFT;
            DtaTime.Font = new Font(Font.FontFamily.HELVETICA, 8.5f, Font.BOLD);
            DtaTime.Add("FECHA: 12/06/2023 12:00:00");
            tablaDataTime.AddCell(DtaTime);

            Paragraph DtaCaja = new Paragraph();
            DtaCaja.Leading = 8;
            DtaCaja.Alignment = Element.ALIGN_RIGHT;
            DtaCaja.Font = new Font(Font.FontFamily.HELVETICA, 8.5f, Font.BOLD);
            DtaCaja.Add("USU: CAJA");
            tablaDataTime.AddCell(DtaCaja);
            tablaDataTime.SpacingAfter = 5;

            //TABLA Add Data Time
            PdfPTable tablaCaja = new PdfPTable(2);
            tablaCaja.DefaultCell.Padding = 0;
            tablaCaja.DefaultCell.Border = 0;
            tablaCaja.SetWidths(new float[] { 1f, 1f});
            tablaCaja.WidthPercentage = 100f;

            Paragraph DtaEstacion = new Paragraph();
            DtaEstacion.Leading = 8;
            DtaEstacion.Alignment = Element.ALIGN_LEFT;
            DtaEstacion.Font = new Font(Font.FontFamily.HELVETICA, 8.5f, Font.BOLD);
            DtaEstacion.Add("ESTACION: CAJA1");
            tablaCaja.AddCell(DtaEstacion);

            Paragraph DtaVendedor = new Paragraph();
            DtaVendedor.Leading = 8;
            DtaVendedor.Alignment = Element.ALIGN_RIGHT;
            DtaVendedor.Font = new Font(Font.FontFamily.HELVETICA, 8.5f, Font.BOLD);
            DtaVendedor.Add("VENDEDOR: CA3");
            tablaCaja.AddCell(DtaVendedor);

            tablaCaja.SpacingAfter = 5;

            Paragraph Line = new Paragraph(new String('_',30));
            Line.Alignment = Element.ALIGN_LEFT;
            Line.Font = new Font(Font.FontFamily.HELVETICA, 2f, Font.BOLD);

            //add DOC
            doc.Add(pHeader);
            doc.Add(pTicketID);
            doc.Add(pClientInfo);
            doc.Add(tablaDataTime);
            doc.Add(tablaCaja);
            doc.Add(Line);

            Paragraph TextDescripcion = new Paragraph();
            TextDescripcion.SpacingBefore = 2;
            TextDescripcion.SpacingAfter = 5;
            TextDescripcion.Leading = 8;
            TextDescripcion.Alignment = Element.ALIGN_CENTER;
            TextDescripcion.Font = new Font(Font.FontFamily.HELVETICA, 8.5f, Font.BOLD);
            TextDescripcion.Add("D E S C R I P C I O N");
            doc.Add(TextDescripcion);
            //Header Table
            PdfPTable tablaHeaderImport = new PdfPTable(5);
            tablaHeaderImport.DefaultCell.Padding = 0;
            tablaHeaderImport.DefaultCell.Border = 0;
            tablaHeaderImport.SetWidths(new float[] { .8f,.9f,1.4f,.9f,1f });
            tablaHeaderImport.WidthPercentage = 100f;
            var font_Bold = new Font(Font.FontFamily.HELVETICA, 8.5f, Font.BOLD);
            var font_Light = new Font(Font.FontFamily.HELVETICA, 8f, Font.NORMAL);
            tablaHeaderImport.AddCell(GetParagraph("CANT.", 8, font_Bold, Element.ALIGN_LEFT));            
            tablaHeaderImport.AddCell(GetParagraph("  LOTE", 8, font_Bold, Element.ALIGN_LEFT));
            tablaHeaderImport.AddCell(GetParagraph("CADUCIDAD", 8, font_Bold, Element.ALIGN_LEFT));
            tablaHeaderImport.AddCell(GetParagraph("PRECIO", 8, font_Bold, Element.ALIGN_LEFT));
            tablaHeaderImport.AddCell(GetParagraph("IMPORTE", 8, font_Bold, Element.ALIGN_LEFT));                       
            doc.Add(tablaHeaderImport);
            doc.Add(Line);
            foreach (var item in new int[3])
            {
                var name_product = GetParagraph("El nombre del producto es ", 8, font_Light, Element.ALIGN_LEFT);
                name_product.SpacingAfter = 3f;
                name_product.SpacingBefore = 3f;
                doc.Add(name_product);
                PdfPTable tablaProducts = new PdfPTable(5);
                tablaProducts.DefaultCell.Padding = 0;
                tablaProducts.DefaultCell.Border = 0;
                tablaProducts.SetWidths(new float[] { .9f, .9f, 1.4f, 1f, 0.8f });
                tablaProducts.WidthPercentage = 100f;
                tablaProducts.AddCell(GetParagraph("10", 8, font_Light, Element.ALIGN_LEFT));
                tablaProducts.AddCell(GetParagraph($"2250121", 8, font_Light, Element.ALIGN_LEFT));
                tablaProducts.AddCell(GetParagraph($"30/01/26", 8, font_Light, Element.ALIGN_LEFT));
                tablaProducts.AddCell(GetParagraph($"${9.63}", 8, font_Light, Element.ALIGN_LEFT));
                tablaProducts.AddCell(GetParagraph($"${96.35}", 8, font_Light, Element.ALIGN_RIGHT));
                doc.Add(tablaProducts);


                PdfPTable tablaProductsVuelto = new PdfPTable(5);
                tablaProductsVuelto.DefaultCell.Padding = 0;
                tablaProductsVuelto.DefaultCell.Border = 0;
                tablaProductsVuelto.SetWidths(new float[] { .9f, .9f, 1.4f, 1f, 0.8f });
                tablaProductsVuelto.WidthPercentage = 100f;
                tablaProductsVuelto.AddCell(GetParagraph(" ", 8, font_Light, Element.ALIGN_LEFT));
                tablaProductsVuelto.AddCell(GetParagraph($" ", 8, font_Light, Element.ALIGN_LEFT));
                tablaProductsVuelto.AddCell(GetParagraph($" ", 8, font_Light, Element.ALIGN_LEFT));
                tablaProductsVuelto.AddCell(GetParagraph($" ", 8, font_Light, Element.ALIGN_LEFT));
                tablaProductsVuelto.AddCell(GetParagraph($"$00.00", 8, font_Light, Element.ALIGN_RIGHT));
                doc.Add(tablaProductsVuelto);

            }
            doc.Add(Line);
            PdfPTable tablaFooter = new PdfPTable(4);
            tablaFooter.DefaultCell.Padding = 0;
            tablaFooter.DefaultCell.Border = 0;
            tablaFooter.SetWidths(new float[] { .3f, 1.4f, 2.1f, .5f, });
            tablaFooter.WidthPercentage = 100f;
            tablaFooter.AddCell(GetParagraph($"{16}", 8, font_Light, Element.ALIGN_LEFT));
            tablaFooter.AddCell(GetParagraph("PRODUCTOS", 8, font_Light, Element.ALIGN_LEFT));
            tablaFooter.AddCell(GetParagraph("TOTAL", 8, font_Hight, Element.ALIGN_LEFT));
            tablaFooter.AddCell(GetParagraph($"${100.00}", 8, font_Hight, Element.ALIGN_RIGHT));
            tablaFooter.SpacingBefore = 5f;
            tablaFooter.SpacingAfter = 5f;
            doc.Add(tablaFooter);
            var numberToText = GetParagraph("TRESCIENTOS SESENTA Y CINCO PESOS (74/100) MN", 8, font_Light, Element.ALIGN_CENTER);
            numberToText.SpacingBefore = 3f;
            numberToText.SpacingAfter = 3f;
            doc.Add(numberToText);


            PdfPTable tablaPago = new PdfPTable(4);
            tablaPago.DefaultCell.Padding = 0;
            tablaPago.DefaultCell.Border = 0;
            tablaPago.SetWidths(new float[] { 1.3f, 1f,1f, 0.7f });
            tablaPago.WidthPercentage = 100f;
            tablaPago.AddCell(GetParagraph($" ", 8, font_Bold, Element.ALIGN_LEFT));
            tablaPago.AddCell(GetParagraph("Su Pago:", 8, font_Bold, Element.ALIGN_LEFT));
            tablaPago.AddCell(GetParagraph("EFE", 8, font_Light, Element.ALIGN_LEFT));
            tablaPago.AddCell(GetParagraph($"$0.00", 8, font_Light, Element.ALIGN_RIGHT));
            tablaPago.SpacingBefore = 3f;
            tablaPago.SpacingAfter = 3f;
            doc.Add(tablaPago);

            PdfPTable tablaPagoTCR = new PdfPTable(4);
            tablaPagoTCR.DefaultCell.Padding = 0;
            tablaPagoTCR.DefaultCell.Border = 0;
            tablaPagoTCR.SetWidths(new float[] { 1.3f, 1f, 1f, 0.7f });
            tablaPagoTCR.WidthPercentage = 100f;
            tablaPagoTCR.AddCell(GetParagraph($" ", 8, font_Bold, Element.ALIGN_LEFT));
            tablaPagoTCR.AddCell(GetParagraph(" ", 8, font_Bold, Element.ALIGN_LEFT));
            tablaPagoTCR.AddCell(GetParagraph("TCR", 8, font_Light, Element.ALIGN_RIGHT));
            tablaPagoTCR.AddCell(GetParagraph($"$555.00", 8, font_Light, Element.ALIGN_RIGHT));
            tablaPagoTCR.SpacingBefore = 3f;
            tablaPagoTCR.SpacingAfter = 3f;
            doc.Add(tablaPagoTCR);


            PdfPTable tablaPagoTCRVuelto = new PdfPTable(4);
            tablaPagoTCRVuelto.DefaultCell.Padding = 0;
            tablaPagoTCRVuelto.DefaultCell.Border = 0;
            tablaPagoTCRVuelto.SetWidths(new float[] { 1.3f, 1f, 1f, 0.7f });
            tablaPagoTCRVuelto.WidthPercentage = 100f;
            tablaPagoTCRVuelto.AddCell(GetParagraph($" ", 8, font_Bold, Element.ALIGN_LEFT));
            tablaPagoTCRVuelto.AddCell(GetParagraph(" ", 8, font_Bold, Element.ALIGN_LEFT));
            tablaPagoTCRVuelto.AddCell(GetParagraph(" ", 8, font_Light, Element.ALIGN_RIGHT));
            tablaPagoTCRVuelto.AddCell(GetParagraph($"$00.00", 8, font_Light, Element.ALIGN_RIGHT));
            tablaPagoTCRVuelto.SpacingBefore = 3f;
            tablaPagoTCRVuelto.SpacingAfter = 3f;
            doc.Add(tablaPagoTCRVuelto);


            PdfPTable tablaCambio = new PdfPTable(4);
            tablaCambio.DefaultCell.Padding = 0;
            tablaCambio.DefaultCell.Border = 0;
            tablaCambio.SetWidths(new float[] { 1.3f, 1f, 1f, 0.7f });
            tablaCambio.WidthPercentage = 100f;
            tablaCambio.AddCell(GetParagraph($" ", 8, font_Bold, Element.ALIGN_LEFT));
            tablaCambio.AddCell(GetParagraph(" ", 8, font_Bold, Element.ALIGN_LEFT));
            tablaCambio.AddCell(GetParagraph("Su cambio:", 8, font_Bold, Element.ALIGN_RIGHT));
            tablaCambio.AddCell(GetParagraph($"$00.00", 8, font_Bold, Element.ALIGN_RIGHT));
            tablaCambio.SpacingBefore = 3f;
            tablaCambio.SpacingAfter = 3f;
            doc.Add(tablaCambio);


            Paragraph pFinal = new Paragraph();
            pFinal.Leading = 9;
            pFinal.Alignment = Element.ALIGN_CENTER;
            pFinal.SpacingBefore = 5;
            Chunk chunkpFinal = new Chunk("UNTOS COMPROMETIDOS CON LA SALUD\n"+
                                            "LAS CONDICIONES DE ALMACENAMIENTO\n"+
                                            "DEBEN SER DE ACUERDO CON LAS\n"+
                                            "ESPECIFICACIONES DEL FABRICANTE\n"+
                                            "NO SE ACEPTAN DEVOLUCIONES\n"+
                                            "DOCUMENTOS SIN EFECTOS FISCALES\n"+
                                            "EN CASO DE REQUERIR FACTURA\n"+
                                            "SOLICITELA EL MISMO DIA");
            chunkpFinal.Font = new Font(Font.FontFamily.HELVETICA, 8.5f, Font.BOLD);
            pFinal.Add(chunkpFinal);
            doc.Add(pFinal);

            Anchor miEnlace = new Anchor("www.elpunto.com.mx", new Font(Font.FontFamily.TIMES_ROMAN, 11f));
            miEnlace.Reference = "https://www.elpunto.com.mx/";
            Paragraph miParrafo = new Paragraph();
            miParrafo.Alignment = Element.ALIGN_CENTER;
            miParrafo.SpacingBefore = 5;
            miParrafo.SpacingAfter = 9;
            miParrafo.Add(miEnlace);

            doc.Add(miParrafo);
            doc.Add(GetParagraph("Gracias por su visita", 8, font_Bold, Element.ALIGN_CENTER));


            doc.Close();
        }
        private Paragraph GetParagraph(string txt, float leading, Font font, int align)
        {
            Paragraph temp = new Paragraph();
            temp.Leading = leading;
            temp.Alignment = align;
            temp.Font = font;
            temp.Add(txt);
            return temp;
        }
    }
}
