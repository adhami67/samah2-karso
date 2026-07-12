using System.ComponentModel.DataAnnotations;
using IRIT.Framework.Resources;
using IRIT.Framework.Utility.Filters;
using System.Web;

namespace IRIT.EdMS.Workflow.Model.ViewModel.WorkType
{
    public class WorkTypeViewModel
    {
        #region Id
        public long Id { get; set; }
        #endregion

        #region Code
        [Display(Name = "Code", ResourceType = typeof (GeneralResource))]
        [StringLength(256, MinimumLength = 3, ErrorMessageResourceType = typeof (GeneralResource),
            ErrorMessageResourceName = "StringLength3To256")]
        public string WorkTypeCode { get; set; }
        #endregion

        #region WorkTypeTitle
        [Required(ErrorMessageResourceName = "RequiredMessage", ErrorMessageResourceType = typeof (GeneralResource))]
        [Display(Name = "Title", ResourceType = typeof (GeneralResource))]
        [StringLength(256, MinimumLength = 3, ErrorMessageResourceType = typeof (GeneralResource),ErrorMessageResourceName = "StringLength3To256")]
        public string WorkTypeTitle { get; set; }
        #endregion

        #region WorkTypeColor
        [Required(ErrorMessageResourceName = "RequiredMessage", ErrorMessageResourceType = typeof(GeneralResource))]
        [Display(Name = "Color", ResourceType = typeof(GeneralResource))]
        [UIHint("ColorPicker")]
        public int WorkTypeColor { get; set; }
        #endregion

        #region Image
        [Required(ErrorMessageResourceName = "RequiredMessage", ErrorMessageResourceType = typeof(GeneralResource))]
        [FileType("jpg,png", ErrorMessageResourceName = "ImageTypeMessage", ErrorMessageResourceType = typeof(GeneralResource))]
        [MaxFileSize(1, ErrorMessageResourceName = "ImageSizeMessage", ErrorMessageResourceType = typeof(GeneralResource))]
        
        public HttpPostedFileBase Image { get; set; }
        public byte[] ImageByte { get; set; }
        #endregion

        #region  Comment
        [Display(Name = "Comment", ResourceType = typeof(GeneralResource))]
        public string Comment { get; set; }
        #endregion
    }

}
