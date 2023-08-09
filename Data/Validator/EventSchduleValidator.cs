
using System;
using System.ComponentModel.DataAnnotations;
using mvc.Models.Event;
using System.Linq;
using System.Reflection;

namespace mvc.Data.Validator
{

    public class StartDate : ValidationAttribute
    {

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            PropertyInfo endDateProperty = validationContext.ObjectType.GetProperty("End");
            var endDate = endDateProperty.GetValue(validationContext.ObjectInstance,null);
            if((DateTime) value < (DateTime)endDate){
                return ValidationResult.Success;
            }
            return new ValidationResult("Start Date Must After Previous Schedule End");

        }
        

    }
    public class EventSchduleEndValidator : ValidationAttribute
    {

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var model = (Models.Event.EventSchedule) validationContext.ObjectInstance;
            var Start = model.Start;
            Console.WriteLine("===========");
            Console.WriteLine(Start);
            Console.WriteLine(value);
            Console.WriteLine("===========");
            if((DateTime)value == DateTime.MinValue){
                return new ValidationResult("End Date Cannot Be Empty");
            }
            if((DateTime)value != DateTime.MinValue && Start>(DateTime)value){
                return new ValidationResult(".Success");
            }
            return new ValidationResult("End Date Must After Schedule Start");
        }
        

    }
    
}