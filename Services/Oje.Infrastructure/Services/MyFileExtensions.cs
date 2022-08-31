using System.Collections.Generic;

namespace Oje.Infrastructure.Services
{
    public class MyFileExtensions
    {
        public static Dictionary<string, byte[]> allExtensions = new Dictionary<string, byte[]>()
        {
            { ".bmp", new byte[] { 66, 77 }  },
            { ".doc", new byte[] { 208, 207, 17, 224, 161, 177, 26, 225 }  },
            { ".exe", new byte[] { 77, 90 }  },
            { ".dll", new byte[] { 77, 90 }  },
            { ".gif", new byte[] { 71, 73, 70, 56 }  },
            { ".ico", new byte[] { 0, 0, 1, 0 }  },
            { ".jpg", new byte[] { 255, 216, 255 }  },
            { ".jpeg", new byte[] { 255, 216, 255 }  },
            { ".mp3", new byte[] { 73, 68, 51 }  },
            { ".ogg", new byte[] { 79, 103, 103, 83, 0,  }  },
            { ".png", new byte[] { 137, 80, 78, 71, 13, 10, 26, 10, 0, 0, 0, 13, 73, 72, 68, 82 }  },
            { ".rar", new byte[] { 82, 97, 114, 33, 26, 7, 0 }  },
            { ".swf", new byte[] { 70, 87, 83 }  },
            { ".tiff", new byte[] {  73, 73, 42, 0 }  },
            { ".torrent", new byte[] { 100, 56, 58, 97, 110, 110, 111, 117, 110, 99, 101 }  },
            { ".ttf", new byte[] { 0, 1, 0, 0, 0 }  },
            { ".wav", new byte[] { 82, 73, 70, 70 }  },
            { ".avi", new byte[] { 82, 73, 70, 70 }  },
            { ".wmv", new byte[] {  48, 38, 178, 117, 142, 102, 207, 17, 166, 217, 0, 170, 0, 98, 206, 108 }  },
            { ".wma", new byte[] {  48, 38, 178, 117, 142, 102, 207, 17, 166, 217, 0, 170, 0, 98, 206, 108 }  },
            { ".zip", new byte[] {  80, 75, 3, 4 }  },
            { ".docx", new byte[] {  80, 75, 3, 4 }  },
            { ".pdf", new byte[] { 37, 80, 68, 70 }  },
            { ".xlsx", new byte[] { 80, 75, 3, 4 }  },
            { ".svg", new byte[] { 60, 115, 118, 103 }  },
            { ".mp4", new byte[] { 0, 0, 0, 32, 102, 116, 121, 112 }  },
            { ".webm", new byte[] { 26, 69, 223, 163, 159 }  }
        };
    }
}
