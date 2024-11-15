using AutoMapper;
using Domain.DTO;
using Domain.Entities;
using PhoneNumbers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOMapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<RegisterDto,ApplicationUser>();

        }

        //private string? FormatPhoneNumber(string? phoneNumber)
        //{
        //    if (phoneNumber is null || string.IsNullOrEmpty(phoneNumber)) return null;
        //    PhoneNumberUtil? phoneUtil = PhoneNumberUtil.GetInstance();
        //    PhoneNumber number = phoneUtil.Parse(phoneNumber, "IN"); // Default country code if not provided
        //    if (!phoneUtil.IsValidNumber(number))
        //        throw new Exception("Invalid phone number.");

        //    return phoneUtil.Format(number, PhoneNumberFormat.E164);
        //}
    }
}
