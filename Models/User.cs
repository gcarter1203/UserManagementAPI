using System.ComponentModel.DataAnnotations;

namespace UserManagementAPI.Models
{
    public class User
    {
        public int Id {get; set;}
        
        [Required]
        public string UserName {get; set;} = string.Empty;

        [Required]
        [EmailAddress]
        public string UserEmail {get; set;} = string.Empty; //makes sure data type is an email. "not-an-email" wont work
    }
}

//copilot suggested a model folder to store user in to make it easier to manage
