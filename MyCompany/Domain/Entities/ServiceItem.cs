using System.ComponentModel.DataAnnotations;

namespace MyCompany.Domain.Entities
{
    public class ServiceItem : EntityBase
    {
        [Required(ErrorMessage = "Заполните название айтема")]
        [Display(Name = "Название айтема")]
        public override string Title { get; set; }

        [Display(Name = "Краткое описание айтема")]
        public override string Subtitle { get; set; }

        [Display(Name = "Полное описание айтема")]
        public override string Text { get; set; }


    }
}
