using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Kodasat.CustomValidation
{
    public class DepositeLessCountAvailable : ValidationAttribute
    {
        private int av;
        private int pd;
        public DepositeLessCountAvailable(int available,int peopleDeposited) :base("يجب ان يكون عدد الأشخاص فى القداس أكبر من "+peopleDeposited)
        {
            av = available;
            pd = peopleDeposited;
        }
        public override bool IsValid(object value)
        {

            if (av < pd)
                return false;
            else
                return true;
        }
    }
}