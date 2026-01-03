using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateHub.Application.Exceptions
{
    public class ValidationException:Exception
    {
        public IDictionary<string, string[]> Errors { get; }

        public ValidationException()
                 : base("One or more validation failures have occurred.")
        {
            Errors = new Dictionary<string, string[]>();
        }
        public ValidationException(IEnumerable<FluentValidation.Results.ValidationFailure> failures)
        {
            Errors = failures
                .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
                .ToDictionary(g => g.Key, g => g.ToArray());
        }
    }
}
