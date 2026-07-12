using System.ComponentModel.DataAnnotations;
using IRIT.Framework.Resources;

namespace IRIT.EdMS.Common.Model.ViewModel.PartyOwner
{
    public class PartyOwnerViewModel
    {
        #region Id
        public long Id { get; set; }
        #endregion

        #region OwnerId
        [Display(Name = "School", ResourceType = typeof(GeneralResource))]
        public long OwnerId { get; set; }
        #endregion
    }
}
