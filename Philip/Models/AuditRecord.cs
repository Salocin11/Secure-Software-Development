using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Philip.Models
{
    public class AuditRecord
    {
        [Key]
        public int Audit_ID { get; set; }

        [Display(Name = "Audit Action")]
        public string AuditActionType { get; set; }
        // Could be  Login Success /Failure/ Logout, Create, Delete, View, Update

        [Display(Name = "Performed By")]
        public string Username { get; set; }
        //Logged in user performing the action

        [Display(Name = "Date/Time Stamp")]
        [DataType(DataType.DateTime)]
        public DateTime DateTimeStamp { get; set; }
        //Time when the event occurred

        [Display(Name = "Post Record ID ")]
        public int KeyPostFieldID { get; set; }
        //Store the ID of post record that is affected


        //trying stuff here
        [Display(Name = "Old Value")]
        public string OldValue { get; set; }
        //Old Value of content

        [Display(Name = "New Value")]
        public string NewValue { get; set; }
        //New value of content
    }

}
