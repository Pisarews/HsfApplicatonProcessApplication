using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Schema;
using FluentValidation;
using FluentValidation.Validators;
using Hsf.ApplicatonProcess.August2020.Data___.NetStandard_2._1_Class_Library;
using RestSharp;

namespace Hsf.ApplicatonProcess.August2020.Domain
{
    public class NameApplicantValidator:AbstractValidator<Applicant>
    {
        public NameApplicantValidator()
        {
            RuleFor(x => x.Name).NotNull().Length(5, 255);
        }
    }

    public class FamilyNameApplicantValidator : AbstractValidator<Applicant>
    {
        public FamilyNameApplicantValidator()
        {
            RuleFor(x => x.FamilyName).NotNull().Length(5, 255);
        }
    }

    public class AddressApplicantValidator : AbstractValidator<Applicant>
    {
        public AddressApplicantValidator()
        {
            RuleFor(x => x.Address).NotNull().Length(10, 255);
        }
    }

    public class CountryValidator : AsyncValidatorBase
    {
        private readonly IRestClient _restClient; 

        public CountryValidator(IRestClient restClient) : base("Country name '{PropertyValue}' is not valid.")
        {
            _restClient = restClient; 
        }
        protected override async Task<bool> IsValidAsync(PropertyValidatorContext context, CancellationToken cancellation)
        { 
            return (await _restClient.GetAsync($"https://restcountries.eu/rest/v2/name/{context.PropertyValue}?fullText=true")).IsSuccessStatusCode;
        }
    }

    public class CountryApplicantValidator : AbstractValidator<Applicant>
    {
        public CountryApplicantValidator(IRestClient restClient)
        {
            RuleFor(x => x.CountryOfOrigin).NotNull().SetValidator(new CountryValidator(restClient)).WithMessage("Country Name is Not Valid !");
        }
    }

    public class EMailAdressApplicantValidator : AbstractValidator<Applicant>
    {
        public EMailAdressApplicantValidator()
        {
            RuleFor(x => x.EMailAdress).NotNull().EmailAddress(); 
        }
    }

    public class AgeApplicantValidator : AbstractValidator<Applicant>
    {
        public AgeApplicantValidator()
        {
            RuleFor(x => x.Age).NotNull().InclusiveBetween(20, 60);
        }
    }

    public class HiredApplicantValidator : AbstractValidator<Applicant>
    {
        public HiredApplicantValidator()
        {
            RuleFor(x => x.Hired).NotNull(); 
        }
    }

}
