using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows.Media.Imaging;
using WebSupergoo.ABCpdf12;

namespace ETG.ABCPdf
{
    public class PDFDoc
    {
        private Doc _doc;
        public PDFDoc(Doc doc)
        {
            _doc = doc;
        }
        public void SetDocPosition(string fieldName)
        {
            var field = _doc.Form.Fields.Where(a => a.Name == fieldName).FirstOrDefault();

            if (field == null)
            {
                return;
            }

            _doc.Page = field.PageID;
            _doc.Rect.Position(field.Rect.Left, field.Rect.Top - field.Rect.Height);
            _doc.Rect.Height = field.Rect.Height;
            _doc.Rect.Width = field.Rect.Width;
        }

        public void StampField(string fieldName)
        {
            var field = _doc.Form.Fields.Where(a => a.Name == fieldName).FirstOrDefault();

            if (field == null)
            {
                return;
            }
            field.Stamp();
        }

        public void SetPDFFieldHTML(string fieldName, string value, int size, int page = 0)
        {
            if (page > 0)
            {
                _doc.Page = page;
            }
            var field = _doc.Form.Fields.Where(a => a.Name == fieldName).FirstOrDefault();

            if (field == null)
            {
                return;
            }

            
            _doc.Rect.Position(field.Rect.Left, field.Rect.Top - field.Rect.Height);
            _doc.Rect.Height = field.Rect.Height;
            _doc.Rect.Width = field.Rect.Width;
            if (size > 0)
            {

                _doc.AddTextStyled($"<stylerun fontsize=\"{size}\">{value}</stylerun>");
            }
            else
            {
                _doc.AddTextStyled(value);

            }
            field.Stamp();
        }

        public int SetPDFHTML(string value, double width, double height, int size, int page = 0)
        {
            if (page > 0)
            {
                _doc.Page = page;
            }

            if (height > 0)
            {
                _doc.Rect.Height = height;
            }

            if (width > 0)
            {
                _doc.Rect.Width = width;
            }
                
            if (size > 0)
            {

                return _doc.AddTextStyled($"<stylerun fontsize=\"{size}\">{value}</stylerun>");
            }
            else
            {
                return _doc.AddTextStyled(value);

            }
        }

        public void SetPDFFieldText(string fieldName, string value, int page = 0)
        {
            if (page > 0)
            {
                _doc.Page = page;
            }
            var field = _doc.Form.Fields.Where(a => a.Name == fieldName).FirstOrDefault();

            if (field == null)
            {
                return;
            }
            
            field.Value = value;
            field.Stamp();
        }

        public void AddRowLine(double y)
        {
            _doc.Color.String = "204 204 204";
            _doc.AddLine(36,
                y,
                _doc.MediaBox.Width - 36,
                y);
            _doc.Color.String = "0 0 0";
        }

        public void AddImage(int page, string fieldName, string imagePath)
        {
            double posX = 0, posY = 0, width = 0, height = 0;


            _doc.PageNumber = page;
            var f = _doc.Form.Fields.Where(a => a.Name == fieldName).FirstOrDefault();

            if (f != null)
            {

                if (f.Name.ToLower() == fieldName.ToLower())
                {
                    posX = f.Rect.Left;
                    posY = f.Rect.Top;
                    height = f.Rect.Height;
                    width = f.Rect.Width;
                    f.Stamp();

                    if (imagePath != string.Empty)
                    {
                        try
                        {
                            XImage objImage = new XImage();

                            objImage.SetFile(imagePath);

                            _doc.Rect.Position(posX, posY - height);
                            _doc.Rect.Height = height;
                            _doc.Rect.Width = width;
                            _doc.AddImageObject(objImage, true);
                            objImage.Dispose();
                        }
                        catch
                        {
                        }
                    }
                    return;
                }
            }
        }

        public void AddImageUrl(int page, double left, double top, double height, string imageUrl)
        {
            try
            {
                var client = new WebClient();
                var bytes = client.DownloadData(imageUrl);

                using (MemoryStream ms = new MemoryStream(bytes))
                {
                    var image = XImage.FromStream(ms, new XReadOptions());
                    _doc.Rect.Position(left, top - height);
                    _doc.Rect.Height = height;
                    _doc.Rect.Width = image.Width * height / image.Height;
                    _doc.AddImageObject(image, true);
                    
                }
            }
            catch
            {
            }
        }
    }
}
