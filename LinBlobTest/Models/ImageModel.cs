using System.ComponentModel.DataAnnotations;

namespace LinBlobTest.Models
{
    public class ImageModel
    {
        [Display(Name = "Blob Name")] 
        public string BlobName { get; set; }
        public byte[]? ImageBytes { get; set; }

        public string ImageDataUri
        {
            get
            {
                if (this.ImageBytes != null)
                {
                    string dataString = Convert.ToBase64String(this.ImageBytes);
                    return $"data:image/jpeg;base64,{dataString}";
                }
                else { return string.Empty; } 
            }
        }
    }
}
