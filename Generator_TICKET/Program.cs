using System;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace Generator_TICKET
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Generate PDF: ");
            bool success = false;
            try
            {
                string clientInfo = "100524 - MARIA GUADALUPE AGUILARA MARIN";
                DataTicket dataTicket = new DataTicket();
                dataTicket.id = 1234;
                dataTicket.client.id = 100524;
                dataTicket.client.name = "MARIA GUADALUPE AGUILARA MARIN";
                dataTicket.distribuidor.fecha = "12/06/2023";
                dataTicket.distribuidor.hora = "12:00:00";
                dataTicket.distribuidor.USU = "CAJA";
                dataTicket.distribuidor.Estacion = "CAJA1";
                dataTicket.productos = new List<Producto>(); //falta el FOR para pintar en la tabla
                dataTicket.pago.totalProductos = 20;
                dataTicket.pago.totalPrecio = 58;
                dataTicket.pago.pagoSTR = "TRESCIENTOS SESENTA Y CINCO PESOS (74/100) MN";
                dataTicket.pago.EFE = 50.50;
                dataTicket.pago.TCR = 60.50;
                dataTicket.pago.EFETCR = 70.50;
                dataTicket.pago.cambio = 80.50;


                FacturaCompraPDF factura = new FacturaCompraPDF();
                factura.CrearFacturaCompraPDF(dataTicket);
                success = true;
            }
            catch (Exception e)
            {

                Console.Write("Fail");

            }

            if (success)
            {
                Console.Write("Succefully");
            }
            
            Console.ReadKey();
        }
    }

    public class FacturaCompraPDF
    {
        public void CrearFacturaCompraPDF(DataTicket dataticket)
        {
            // Crear documento:
            Document doc = new Document(new Rectangle(223.94f, 800), 10, 10, 10, 10);
            PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream($"Ticket_{dataticket.id}.pdf", FileMode.Create));
            
            doc.Open();

            //FONTS
            var font_Hight = new Font(Font.FontFamily.HELVETICA, 10, Font.BOLD);
            var font_VeryHight = new Font(Font.FontFamily.HELVETICA, 12, Font.BOLD);
            var font_MidlleHight = new Font(Font.FontFamily.HELVETICA, 8.5f, Font.BOLD);
            var font_Bold = new Font(Font.FontFamily.HELVETICA, 8.5f, Font.BOLD);
            var font_Light = new Font(Font.FontFamily.HELVETICA, 8f, Font.NORMAL);
            //var font_Line = new Font(Font.FontFamily.HELVETICA, 2f, Font.BOLD);

            // Image Logo
            string rutaImagenEncabezado = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "assets", "elPunto.png");
            Image imagenEncabezado = Image.GetInstance(rutaImagenEncabezado);
            imagenEncabezado.Alignment = Element.ALIGN_CENTER;
            doc.Add(imagenEncabezado);

            // HEADER
            doc.Add(GetParagraph(GetChunk($"DISTRIBUIDORA FARMACEUTICA\nPERU #100 ESQ. 5 DE MAYO, TOLUCA MEX.\nC.P. 50090 SUC.TOLUCA\nCEL / WHATSAPP 722 5850951 / 722 3985672", font_MidlleHight), 9));
            
            // TicketID
            doc.Add(GetParagraph($"TICKET: {dataticket.id}",9,9,9, font_VeryHight, Element.ALIGN_CENTER));

            // ClientInfo
            doc.Add(GetParagraph(GetChunk($"CLIENTE: {dataticket.client.id} - {dataticket.client.name}", font_MidlleHight),0,5,8,Element.ALIGN_LEFT));

            // TABLA DISTRIBUIDOR SUP. FECHA - HORA / USU
            PdfPTable tblDistribuidor_Sup = GetTable(2, new float[] { 2f, 1f });            
            tblDistribuidor_Sup.AddCell(GetParagraph($"FECHA: {dataticket.distribuidor.fecha} {dataticket.distribuidor.hora}", 8, font_MidlleHight, Element.ALIGN_LEFT));
            tblDistribuidor_Sup.AddCell(GetParagraph($"USU: {dataticket.distribuidor.USU}", 8, font_MidlleHight, Element.ALIGN_LEFT));
            tblDistribuidor_Sup.SpacingAfter = 5;
            doc.Add(tblDistribuidor_Sup);
            
            // TABLA DISTRIBUIDOR INF. ESTACION - VENDEDOR
            PdfPTable tblDistribuidor_inf = GetTable(2, new float[] { 1f, 1f });
            tblDistribuidor_inf.AddCell(GetParagraph($"ESTACION: {dataticket.distribuidor.Estacion} {dataticket.distribuidor.hora}", 8, font_MidlleHight, Element.ALIGN_LEFT));
            tblDistribuidor_inf.AddCell(GetParagraph($"VENDEDOR: {dataticket.distribuidor.vendedor} {dataticket.distribuidor.hora}", 8, font_MidlleHight, Element.ALIGN_LEFT));
            tblDistribuidor_inf.SpacingAfter = 5;
            doc.Add(tblDistribuidor_inf);

            doc.Add(CreateLine());
            
            //DESCRIPTION
            Paragraph TextDescripcion = GetParagraph("D E S C R I P C I O N",8,font_MidlleHight, Element.ALIGN_CENTER);
            TextDescripcion.SpacingBefore = 2;
            TextDescripcion.SpacingAfter = 5;
            doc.Add(TextDescripcion);
            //TABLE PRODUCTS HEADER
            PdfPTable tablaHeaderImport = GetTable(5, new float[] { .8f, .9f, 1.4f, .9f, 1f });
            
            //Header Table           
            tablaHeaderImport.AddCell(GetParagraph("CANT.", 8, font_Bold, Element.ALIGN_LEFT));
            tablaHeaderImport.AddCell(GetParagraph("  LOTE", 8, font_Bold, Element.ALIGN_LEFT));
            tablaHeaderImport.AddCell(GetParagraph("CADUCIDAD", 8, font_Bold, Element.ALIGN_LEFT));
            tablaHeaderImport.AddCell(GetParagraph("PRECIO", 8, font_Bold, Element.ALIGN_LEFT));
            tablaHeaderImport.AddCell(GetParagraph("IMPORTE", 8, font_Bold, Element.ALIGN_LEFT));
            doc.Add(tablaHeaderImport);
            doc.Add(CreateLine());


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
            doc.Add(CreateLine());

            PdfPTable tablaFooter = GetTable(4,new float[] { .3f, 1.4f, 2.1f, .5f, });
            tablaFooter.AddCell(GetParagraph($"{dataticket.pago.totalProductos}", 8, font_Light, Element.ALIGN_LEFT));
            tablaFooter.AddCell(GetParagraph("PRODUCTOS", 8, font_Light, Element.ALIGN_LEFT));
            tablaFooter.AddCell(GetParagraph("TOTAL", 8, font_Hight, Element.ALIGN_LEFT));
            tablaFooter.AddCell(GetParagraph($"${dataticket.pago.totalPrecio}", 8, font_Hight, Element.ALIGN_RIGHT));
            tablaFooter.SpacingBefore = 5f;
            tablaFooter.SpacingAfter = 5f;
            doc.Add(tablaFooter);
            
            // STR Precio
            var numberToText = GetParagraph($"{dataticket.pago.pagoSTR}", 8, font_Light, Element.ALIGN_CENTER);
            numberToText.SpacingBefore = 3f;
            numberToText.SpacingAfter = 3f;
            doc.Add(numberToText);
            
            // SU PAGO / EFE
            PdfPTable tablaPago = GetTable(4,new float[] { 1.3f, 1f, 1f, 0.7f });
            tablaPago.AddCell(GetParagraph($" ", 8, font_Bold, Element.ALIGN_LEFT));
            tablaPago.AddCell(GetParagraph("Su Pago:", 8, font_Bold, Element.ALIGN_LEFT));
            tablaPago.AddCell(GetParagraph("EFE", 8, font_Light, Element.ALIGN_LEFT));
            tablaPago.AddCell(GetParagraph($"${dataticket.pago.EFE}", 8, font_Light, Element.ALIGN_RIGHT));
            tablaPago.SpacingBefore = 3f;
            tablaPago.SpacingAfter = 3f;
            doc.Add(tablaPago);
            
            //TCR
            PdfPTable tablaPagoTCR = GetTable(4,new float[] { 1.3f, 1f, 1f, 0.7f });
            tablaPagoTCR.AddCell(GetParagraph($" ", 8, font_Bold, Element.ALIGN_LEFT));
            tablaPagoTCR.AddCell(GetParagraph(" ", 8, font_Bold, Element.ALIGN_LEFT));
            tablaPagoTCR.AddCell(GetParagraph("TCR", 8, font_Light, Element.ALIGN_RIGHT));
            tablaPagoTCR.AddCell(GetParagraph($"${dataticket.pago.TCR}", 8, font_Light, Element.ALIGN_RIGHT));
            tablaPagoTCR.SpacingBefore = 3f;
            tablaPagoTCR.SpacingAfter = 3f;
            doc.Add(tablaPagoTCR);

            //SUB-TOTAL 
            PdfPTable tablaPagoTCRVuelto = GetTable(4,new float[] { 1.3f, 1f, 1f, 0.7f });
            tablaPagoTCRVuelto.AddCell(GetParagraph($" ", 8, font_Bold, Element.ALIGN_LEFT));
            tablaPagoTCRVuelto.AddCell(GetParagraph(" ", 8, font_Bold, Element.ALIGN_LEFT));
            tablaPagoTCRVuelto.AddCell(GetParagraph(" ", 8, font_Light, Element.ALIGN_RIGHT));
            tablaPagoTCRVuelto.AddCell(GetParagraph($"${dataticket.pago.EFETCR}", 8, font_Light, Element.ALIGN_RIGHT));
            tablaPagoTCRVuelto.SpacingBefore = 3f;
            tablaPagoTCRVuelto.SpacingAfter = 3f;
            doc.Add(tablaPagoTCRVuelto);

           //CAMBIO
            PdfPTable tablaCambio = GetTable(4,new float[] { 1.3f, 1f, 1f, 0.7f });
            tablaCambio.AddCell(GetParagraph($" ", 8, font_Bold, Element.ALIGN_LEFT));
            tablaCambio.AddCell(GetParagraph(" ", 8, font_Bold, Element.ALIGN_LEFT));
            tablaCambio.AddCell(GetParagraph("Su cambio:", 8, font_Bold, Element.ALIGN_RIGHT));
            tablaCambio.AddCell(GetParagraph($"${dataticket.pago.cambio}", 8, font_Bold, Element.ALIGN_RIGHT));
            tablaCambio.SpacingBefore = 3f;
            tablaCambio.SpacingAfter = 3f;
            doc.Add(tablaCambio);


            Paragraph pFinal = new Paragraph();
            pFinal.Leading = 9;
            pFinal.Alignment = Element.ALIGN_CENTER;
            pFinal.SpacingBefore = 5;
            Chunk chunkpFinal = new Chunk("UNTOS COMPROMETIDOS CON LA SALUD\n" +
                                            "LAS CONDICIONES DE ALMACENAMIENTO\n" +
                                            "DEBEN SER DE ACUERDO CON LAS\n" +
                                            "ESPECIFICACIONES DEL FABRICANTE\n" +
                                            "NO SE ACEPTAN DEVOLUCIONES\n" +
                                            "DOCUMENTOS SIN EFECTOS FISCALES\n" +
                                            "EN CASO DE REQUERIR FACTURA\n" +
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

        private Paragraph CreateLine()
        {
            Paragraph Line = new Paragraph(new String('_', 30));
            Line.Alignment = Element.ALIGN_LEFT;
            Line.Font = new Font(Font.FontFamily.HELVETICA, 2f, Font.BOLD);
            return Line;
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

        private PdfPTable GetTable(int columns, float[]relation)
        {
            PdfPTable temp = new PdfPTable(columns);
            temp.DefaultCell.Padding = 0;
            temp.DefaultCell.Border = 0;
            temp.SetWidths(relation);
            temp.WidthPercentage = 100f;
            return temp;
        }


        private Paragraph GetParagraph(string txt,float SpacingBefore,float SpacingAfter, float leading, Font font, int align)
        {
            Paragraph temp = new Paragraph();
            temp.SpacingBefore = 9;
            temp.SpacingAfter = 9;
            temp.Leading = leading;
            temp.Alignment = align;
            temp.Font = font;
            temp.Add(txt);
            return temp;
        }

        private Chunk GetChunk(string txt, Font _font)
        {
            Chunk temp = new Chunk(txt);
            temp.Font = _font;
            return temp;
        }


        private Paragraph GetParagraph(Chunk chunk, float SpacingBefore, float SpacingAfter, float leading, int align)
        {
            Paragraph temp = new Paragraph();
            temp.SpacingBefore = SpacingBefore;
            temp.SpacingAfter = SpacingAfter;
            temp.Leading = leading;
            temp.Alignment = align;
            temp.Add(chunk);
            return temp;
        }

        private Paragraph GetParagraph(Chunk chunk, float leading)
        {
            Paragraph temp = new Paragraph();
            temp.Leading = leading;
            temp.Add(chunk);
            return temp;
        }

    }

    public class Cliente
    {
        public int id;
        public string name;
    }

    public class Distribuidor 
    {
        public string fecha;
        public string hora;
        public string USU;
        public string Estacion;
        public string vendedor;

    }

    public class Producto
    {
        public int cantidad;
        public int lote;
        public string caducidad;
        public double precio;
        public double importe;
    }

    public class Pago
    {
        public int totalProductos;
        public double totalPrecio;
        public string pagoSTR;
        public double EFE;
        public double TCR;
        public double EFETCR;
        public double cambio;
    }

    public class DataTicket
    {
        public int id = 0;
        public Cliente client = new Cliente();
        public Distribuidor distribuidor = new Distribuidor();
        public List<Producto> productos = new List<Producto>();
        public int cantidadTotal = 0;
        public double importe = 0;
        public string importeString = "";
        public Pago pago = new Pago();
    }


}
