using BarcodeDrugCheckerLib.Model.Interface;

namespace BarcodeDrugCheckerLib.Model
{
    public class DrugImage : IDrugImage
    {
        public string ICode { get; set; }
        public string Name { get; set; }
        public byte[] ImageBytes { get; set; }

        /*
        public Bitmap GetBitmapImage
        {
            get
            {
                if(ImageBytes.Length > 0)
                {
                    MemoryStream memStream = new MemoryStream();
                    memStream.Write(ImageBytes, 0, ImageBytes.Length);
                    Bitmap bmImg = new Bitmap(memStream, false);
                    memStream.Close();
                    memStream.Dispose();
                    return bmImg;
                }
                else
                {
                    return null;
                }
            }
        }
        */
    }
}
