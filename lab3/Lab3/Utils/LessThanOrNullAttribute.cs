using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Lab3.Utils {
    public class LessThanOrNullAttribute : ValidationAttribute {
        private int MaxValue { get; }
        public LessThanOrNullAttribute(int maxValue) {
            MaxValue = maxValue;
        }

        public override bool IsValid(object? value) {
            return value is null || value is int v && v < MaxValue;
        }
    }
}
