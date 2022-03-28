using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace TicketCore.ViewModels.MenuCategorys
{
    public class EditCategoriesVM
    {
        public int MenuCategoryId { get; set; }

        [DisplayName("Category Name")]
        [Required(ErrorMessage = "Enter Category Name")]
        public string MenuCategoryName { get; set; }

        [DisplayName("Role")]
        [Required(ErrorMessage = "Select Role")]
        public int RoleId { get; set; }

        [DisplayName("IsActive")]
        [Required(ErrorMessage = "Required IsActive")]
        public bool Status { get; set; }

        public List<SelectListItem> ListofRoles { get; set; }
    }
}