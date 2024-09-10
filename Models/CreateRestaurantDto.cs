using System.ComponentModel.DataAnnotations;

namespace RestaurantAPI.Models
{
    public class CreateRestaurantDto
    {
        [Required]
        [MaxLength(25, ErrorMessage = "customowy error, długość nie może przekraczać 25 znaków")]
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public bool HasDelivery { get; set; }
        public string? ContactEmail { get; set; }
        public string? ContactNumber { get; set; }

        [Required]
        [MaxLength(50)]
        public string City { get; set; }

        [Required]
        [MaxLength(50)]
        public string Street { get; set; }
        public string PostalCode { get; set; }


        //[CreditCard]
        // Potwierdza, że właściwość ma foramt karty kredytowej.

        //[Compare("otherProperty")]
        // Sprawdza, czy dwie właściwości w model są zgodne.

        //[EmailAddress]
        // Sprawdza, czy właściwość ma format wiadomości e-mail

        //[Phone]
        // Sprawdza, czy właściwość ma foramt numeru telefonu.

        //[Range(minValue, maxValue)]
        // Sprawdza, czy warość właściwości mieści się w określonym zakresie.

        //[RegularExpression(patern)]
        // Sprawdza, czy warotość właściwości pasuje do określonego wyrażenia regularntgo.
    }
}
