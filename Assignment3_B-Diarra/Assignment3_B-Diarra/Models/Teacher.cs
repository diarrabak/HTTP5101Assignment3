﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Assignment3_B_Diarra.Models
{
    //This extended class allows getting teacher information and its teachings
    public class Teacher
    {
        public string firstName;
        public string surName;
        public string employeeNumber;
        public DateTime hireDate;
        public decimal salary;
        public string moduleCode;
        public string moduleName;
        public string startDate;  // String was used here for easy display reason 
        public string finishDate; // String was used here for easy display reason
    }
}