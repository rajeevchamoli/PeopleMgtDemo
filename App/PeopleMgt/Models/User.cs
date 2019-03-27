using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace PeopleMgt.Models
{
    public class User
    {

        [Key]
        public int Id { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(50, ErrorMessage = MessageConstants.EmailLengthErrorMsg)]
        public string Email { get; set; }

        [Required]
        [StringLength(25, ErrorMessage = MessageConstants.FirstNameMaxLengthErrorMsg)]
        [MinLength(2, ErrorMessage = MessageConstants.FirstNameMinLengthErrorMsg)]
        [JsonProperty(PropertyName = "firstname")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(25, ErrorMessage = MessageConstants.LastNameMaxLengthErrorMsg)]
        [MinLength(2, ErrorMessage = MessageConstants.LastNameMinLengthErrorMsg)]
        [JsonProperty(PropertyName = "lastname")]
        public string LastName { get; set; }

        [Range(0, 120, ErrorMessage = MessageConstants.AgeRangeErrorMsg)]
        public int? Age { get; set; }

        [StringLength(100, ErrorMessage = MessageConstants.AddressMaxLengthErrorMsg)]
        public string Address { get; set; }

        [StringLength(100, ErrorMessage = MessageConstants.InterestsMaxLengthErrorMsg)]
        public string Interests { get; set; }

        // 100KB = 100000 chars: ref: http://extraconversion.com/data-storage/kilobytes/kilobytes-to-characters.html
        [StringLength(200000, ErrorMessage = MessageConstants.PictureMaxSizeErrorMsg)]
        public string Picture { get; set; }

    }
}
