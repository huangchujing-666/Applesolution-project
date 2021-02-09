using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Palmary.Loyalty.BO.DataTransferObjects.Media
{
    public class PhotoObject
    {
        public int photo_id;

        public int parent_id;

        public string file_name;

        public string file_extension;

        public int display_order;

        public string name;
        public string caption;

        public int status;

        public Dictionary<int, string> photo_full_path_list = new Dictionary<int, string>();  // image_size_index, photo_full_path

    }

    public class PhotoFullPath
    {
        public string image_size_name;
        public string photo_full_path;
    }
}
