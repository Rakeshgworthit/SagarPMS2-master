﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PMS.Repository.Infrastructure;
using PMS.Database;
namespace PMS.Repository.DataService
{
    public class PaymentDetailsDescriptionService: GenericRepository<PMSEntities, payment_details_description>, IPaymentDetailsDescription
    {
    }
}