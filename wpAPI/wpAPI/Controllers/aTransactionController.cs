
using Microsoft.AspNetCore.Mvc;
using NetCoreHTMLToPDF;
using Newtonsoft.Json;
using NReco.PdfGenerator;
using wpAPI.Models;
using Transaction = wpAPI.Models.Transaction;


namespace wpAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class aTransactionController : ControllerBase
    {

        private readonly WpdbContext _context;
        private readonly IConfiguration _configuration;
        //private readonly IConverter _pdfConverter;

        public aTransactionController(WpdbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        //    _pdfConverter = pdfConverter;
        }
        [HttpPost("transactionList")]
        public async Task<ActionResult> transactionList(long userId, int page,int size, String? search, string branchId)
        {
            try
            {
               /* string test = "asdasdasdasdsad";

                 
                return Ok();*/

                int transactionsCount = _context.Transactions.Where(
                  x =>
                  (search == null ? x.ReferenceNumber.ToLower().Contains("") : (x.ReferenceNumber.ToLower().Contains(search.ToLower()) || x.CustomerCode.Contains(search.ToLower()))) && 
                  x.CreatedByUserId == userId &&
                  x.BranchCode == branchId
                  ).Count();

                List<Transaction> transactions = _context.Transactions.Where(
                    x => 
                    ( search == null? x.ReferenceNumber.ToLower().Contains(""): ( x.ReferenceNumber.ToLower().Contains(search.ToLower()) || x.CustomerCode.Contains(search.ToLower()) )) &&
                  x.CreatedByUserId == userId &&
                  x.BranchCode == branchId
                    )
                        .Skip(page * size)
                        .Take(size).ToList();


              
                


                return Ok(new { transactions, transactionsCount }); 

            }
            catch (Exception e)
            {
                if (e.Message.Contains("InnerException") || e.Message.Contains("inner exception"))
                {

                    return StatusCode(202, "InnerExeption: " + e.InnerException);
                }
                else
                {

                    return StatusCode(202, "Error Message: " + e.Message);
                }
            }
        }

        [HttpPost("customerList")]
        public async Task<ActionResult> customerList(long id, int page,int size, String? search)
        {
            try
            {

                int customerCount = _context.Customers.Where(x => x.Status == true && x.IsDelete == false && (search == null ? x.Name.Contains("") : x.Name.ToLower().Contains(search.ToLower()))).Count();

                List<Customer> customerList = _context.Customers.Where(x=> x.Status == true && x.IsDelete == false && (search == null? x.Name.Contains(""):x.Name.ToLower().Contains(search.ToLower()) || x.Code.ToLower().Contains(search.ToLower()) || x.CompanyName.ToLower().Contains(search.ToLower())))
                        .Skip(page * size)
                        .Take(size).ToList();

                return Ok(new { customerList, customerCount });

            }
            catch (Exception e)
            {
                if (e.Message.Contains("InnerException") || e.Message.Contains("inner exception"))
                {

                    return StatusCode(202, "InnerExeption: " + e.InnerException);
                }
                else
                {
                        
                    return StatusCode(202, "Error Message: " + e.Message);
                }
            }
        }

        [HttpPost("storageType")]
        public async Task<ActionResult> storageType()
        {
            try
            {

                List<Dropdown> dropdown = _context.Dropdowns.Where(x => x.Code == "STORAGETYPE").ToList();

                List<Rate> rate = _context.Rates.Where(x => x.Status == 1).Take(5).ToList();
                int rateCount = _context.Rates.Where(x => x.Status == 1).Count();
                List<TermsAndCondition> termsAndCondition = _context.TermsAndConditions.Where(x => x.Status == 1).ToList();
                int termsAndConditionCount = _context.TermsAndConditions.Where(x => x.Status == 1).Count();

                return Ok(new {  dropdown, rate, rateCount, termsAndCondition, termsAndConditionCount });
            }
            catch (Exception e)
            {
                if (e.Message.Contains("InnerException") || e.Message.Contains("inner exception"))
                {

                    return StatusCode(202, "InnerExeption: " + e.InnerException);
                }
                else
                {

                    return StatusCode(202, "Error Message: " + e.Message);
                }
            }
        }

        [HttpPost("storageTypeEdit")]
        public async Task<ActionResult> storageTypeEdit(int transcId)
        {
            try
            {

                List<Dropdown> dropdown = _context.Dropdowns.Where(x => x.Code == "STORAGETYPE").ToList();

                List<Rate> rate = _context.Rates.Where(x => x.Status == 1).Take(5).ToList();
                int rateCount = _context.Rates.Where(x => x.Status == 1).Count();
                List<TermsAndCondition> termsAndCondition = _context.TermsAndConditions.Where(x => x.Status == 1).ToList();
                int termsAndConditionCount = _context.TermsAndConditions.Where(x => x.Status == 1).Count();

                string notedUser = _context.Users.Where(x => x.Id == transcId).Select(x => x.Fullname).FirstOrDefault();

                return Ok(new { dropdown, rate, rateCount, termsAndCondition, termsAndConditionCount, notedUser });
            }
            catch (Exception e)
            {
                if (e.Message.Contains("InnerException") || e.Message.Contains("inner exception"))
                {

                    return StatusCode(202, "InnerExeption: " + e.InnerException);
                }
                else
                {

                    return StatusCode(202, "Error Message: " + e.Message);
                }
            }
        }

        [HttpPost("notedByList")]
        public async Task<ActionResult> notedByList(int page, int size, string? search)
        {
            try
            {
                string[] searchSplitted = search == null ? null : search.Split(" ");


                int userListCount = _context.Users.Where(x =>
              x.Status == 1 &&
              x.IsVerified == 1 &&
              x.IsDeactivated == 0 &&
              (search == null ? x.Fullname2.Contains("") : (x.Fullname.ToLower().Contains(searchSplitted[0].ToLower()) && x.Fullname.ToLower().Contains(searchSplitted[searchSplitted.Count() - 1].ToLower())))

              ).Count();



                List<User> userList = _context.Users.Where(x => 
                x.Status == 1 &&
                x.IsVerified == 1 &&
                x.IsDeactivated == 0 &&
                (search == null ? x.Fullname2.Contains("") : (x.Fullname.ToLower().Contains(searchSplitted[0].ToLower()) && x.Fullname.ToLower().Contains(searchSplitted[searchSplitted.Count() - 1].ToLower())))

                ).
                     Skip(page * size).
                     Take(size).
                    ToList();


                return Ok(new { userList, userListCount });
            }
            catch (Exception e)
            {
                if (e.Message.Contains("InnerException") || e.Message.Contains("inner exception"))
                {

                    return StatusCode(202, "InnerExeption: " + e.InnerException);
                }
                else
                {

                    return StatusCode(202, "Error Message: " + e.Message);
                }
            }
        }

        [HttpPost("rateList")]
        public async Task<ActionResult> rateList(int page, int size, string? search)
        {
            try
            {
                List<Rate> rate = _context.Rates.
                    Where(x => x.Status == 1 &&(search != null? (x.ChargeCode.Contains(search.ToLower()) || x.ChargeName.ToLower().Contains(search.ToLower()) || x.ChargeCategory.ToLower().Contains(search.ToLower())) :x.ChargeName.Contains("") ) )
                    .Skip(page * size)
                    .Take(size)
                    .ToList();
                int rateCount = _context.Rates.
                    Where(x => x.Status == 1 && (search != null ? (x.ChargeCode.Contains(search.ToLower()) || x.ChargeName.ToLower().Contains(search.ToLower()) || x.ChargeCategory.ToLower().Contains(search.ToLower())) : x.ChargeName.Contains(""))).Count();

                return Ok(new { rate, rateCount });
            }
            catch (Exception e)
            {
                if (e.Message.Contains("InnerException") || e.Message.Contains("inner exception"))
                {

                    return StatusCode(202, "InnerExeption: " + e.InnerException);
                }
                else
                {

                    return StatusCode(202, "Error Message: " + e.Message);
                }
            }
        }

        public class aTransaction
        {
            public string? referenceNumber { get; set; }
            public int? CreatedByUserId { get; set; }
            public string? aTransactionDetail { get; set; }
            public string? aTransactionTerms { get; set; }
        }

        public class aTransactionDetail
        {
            public string? ChargeCode { get; set; }
            public string? Currency { get; set; }
            public string? Rates { get; set; }
            public string? Uom { get; set; }
            public string? Remarks { get; set; }
        }

        public class aTransactionTerms
        {
            public string? TermsAndConditionCode { get; set; }
            public string? TermsAndConditionDescription { get; set; }
            public string? Parameter1 { get; set; }
            public string? Parameter2 { get; set; }
        }

        public class aTransactionEdit
        {
            public Transaction? aTransactionEdits { get; set; }
            public string? aTransactionDetailEdit { get; set; }
            public string? aTransactionTermsEdit { get; set; }
        }


        [HttpPost("addTransactionHeader")]
        public async Task<ActionResult> addTransaction(
            Transaction transaction
            )
        {
            try
            {
                transaction.ReferenceDate = DateTime.Parse(transaction.ReferenceDate).ToString("MM/dd/yyyy");

                StrategicBusinessUnit strategicBusinessUnit = _context.StrategicBusinessUnits.Where(x => x.Code == transaction.Sbucode).FirstOrDefault();

                _context.Transactions.Add(transaction);

                _context.SaveChanges();

                return Ok(transaction.ReferenceNumber);
            }
            catch (Exception e)
            {
                if (e.Message.Contains("InnerException") || e.Message.Contains("inner exception"))
                {

                    return StatusCode(202, "InnerExeption: " + e.InnerException);
                }
                else
                {

                    return StatusCode(202, "Error Message: " + e.Message);
                }
            }
        }

        [HttpPost("addTransaction")]
        public async Task<ActionResult> addTransaction(
            aTransaction transaction
            )
        {
            try
            {



                if (transaction.aTransactionDetail != null)
                {
                    List<aTransactionDetail> transactionDetails = JsonConvert.DeserializeObject<List<aTransactionDetail>>(transaction.aTransactionDetail)!;
                    foreach (aTransactionDetail data in transactionDetails)
                    {
                        _context.TransactionDetails.Add(new TransactionDetail
                        {
                            ReferenceNumber = transaction.referenceNumber,
                            ChargeCode = data.ChargeCode,
                            Currency = data.Currency,
                            Rates = data.Rates,
                            Uom = data.Uom,
                            Remarks = data.Remarks,
                            CreatedByUserId = transaction.CreatedByUserId,
                            CreatedDate = DateTime.Now,

                        });

                    }
                }

                if (transaction.aTransactionTerms != null)
                {

                    List<aTransactionTerms> transactionTerms = JsonConvert.DeserializeObject<List<aTransactionTerms>>(transaction.aTransactionTerms)!;
                    foreach (aTransactionTerms data in transactionTerms)
                    {
                        _context.TransactionTermsConditions.Add(new TransactionTermsCondition
                        {
                            ReferenceNumber = transaction.referenceNumber,
                            TermsAndConditionCode = data.TermsAndConditionCode,
                            TermsAndConditionDescription = data.TermsAndConditionDescription,
                            Parameter1 = data.Parameter1,
                            Parameter2 = data.Parameter2,
                            CreatedByUserId = transaction.CreatedByUserId,
                            CreatedDate = DateTime.Now
                        });

                    }
                }

                _context.SaveChanges();

                return Ok();
            }
            catch (Exception e)
            {
                if (e.Message.Contains("InnerException") || e.Message.Contains("inner exception"))
                {

                    return StatusCode(202, "InnerExeption: " + e.InnerException);
                }
                else
                {

                    return StatusCode(202, "Error Message: " + e.Message);
                }
            }
        }


        [HttpPost("referenceEdit")]
        public async Task<ActionResult> referenceEdit(
           String referenceNumber
           )
        {
            try
            {

                List<TransactionDetail> TransactionDetail = _context.TransactionDetails.Where(x => x.ReferenceNumber == referenceNumber).ToList();
                List<TransactionTermsCondition> TransactionTermsCondition = _context.TransactionTermsConditions.Where(x => x.ReferenceNumber == referenceNumber).ToList();

                return Ok(new { TransactionDetail , TransactionTermsCondition });
            }
            catch (Exception e)
            {
                if (e.Message.Contains("InnerException") || e.Message.Contains("inner exception"))
                {

                    return StatusCode(202, "InnerExeption: " + e.InnerException);
                }
                else
                {

                    return StatusCode(202, "Error Message: " + e.Message);
                }
            }
        }


        [HttpPost("referenceEditSave")]
        public async Task<ActionResult> referenceEditSave(
        aTransactionEdit transactionEdit,int userId
        )
        {
            try
            {
                Transaction transaction = _context.Transactions.Where(x => x.ReferenceNumber == transactionEdit.aTransactionEdits.ReferenceNumber).FirstOrDefault();

                transaction.CustomerCode = transactionEdit.aTransactionEdits.CustomerCode;
                transaction.CustomerName = transactionEdit.aTransactionEdits.CustomerName;
                transaction.CompanyName = transactionEdit.aTransactionEdits.CompanyName;
                transaction.CompanyAddress = transactionEdit.aTransactionEdits.CompanyAddress;
                transaction.ReferenceDate = transactionEdit.aTransactionEdits.ReferenceDate;
                transaction.StorageType = transactionEdit.aTransactionEdits.StorageType;
                transaction.ServiceAddress = transactionEdit.aTransactionEdits.ServiceAddress;
                transaction.Commodity = transactionEdit.aTransactionEdits.Commodity;
                transaction.Remarks = transactionEdit.aTransactionEdits.Remarks;
                transaction.NotedByUserId = transactionEdit.aTransactionEdits.NotedByUserId;
                transaction.ModifiedByUserId = userId;
                transaction.ModifiedDate = DateTime.Now;
                transaction.IsRevised = 1;
                transaction.RevisedCount = transaction.RevisedCount + 1;
                


                if (transactionEdit.aTransactionDetailEdit != null)
                {
                    List<aTransactionDetail> transactionDetails = JsonConvert.DeserializeObject<List<aTransactionDetail>>(transactionEdit.aTransactionDetailEdit)!;

                    List<TransactionDetail> checker = _context.TransactionDetails.Where(x => x.ReferenceNumber == transactionEdit.aTransactionEdits.ReferenceNumber).ToList();


                    for (int index1 = 0; index1 < checker.Count; index1++)
                    {
                        bool isTrue = false;
                 
                        for (int index2 = 0; index2 < transactionDetails.Count; index2++)
                        {
                            if (checker[index1].ChargeCode == transactionDetails[index2].ChargeCode)
                            {
                                checker[index1].Rates = transactionDetails[index2].Rates;
                                checker[index1].Remarks = transactionDetails[index2].Remarks;
                                checker[index1].ModifiedByUserId = userId;
                                checker[index1].ModifiedDate = DateTime.Now;
                                transactionDetails.RemoveAt(index2);
                                isTrue = true;
                            }
                        
                        }

                        if (isTrue == false)
                        {
                            _context.TransactionDetails.Remove(checker[index1]);

                        }

                    }

                    if(transactionDetails.Count != 0)
                    {
                        foreach (aTransactionDetail data in transactionDetails)
                        {
                            _context.TransactionDetails.Add(new TransactionDetail
                            {
                                ReferenceNumber = transactionEdit.aTransactionEdits.ReferenceNumber,
                                ChargeCode = data.ChargeCode,
                                Currency = data.Currency,
                                Rates = data.Rates,
                                Uom = data.Uom,
                                Remarks = data.Remarks,
                                CreatedByUserId = transactionEdit.aTransactionEdits.CreatedByUserId,
                                CreatedDate = DateTime.Now,

                            });

                        }
                    }


                }
                else
                {
                    List<TransactionDetail> checker = _context.TransactionDetails.Where(x => x.ReferenceNumber == transactionEdit.aTransactionEdits.ReferenceNumber).ToList();
                    if(checker.Count != 0)
                    {
                        foreach (TransactionDetail data in checker)
                        {
                            _context.TransactionDetails.Remove(data);
                        }
                    }

                }


                if (transactionEdit.aTransactionTermsEdit != null) {

                    List<aTransactionTerms> transactionTerms = JsonConvert.DeserializeObject<List<aTransactionTerms>>(transactionEdit.aTransactionTermsEdit)!;
                    List<TransactionTermsCondition> checker = _context.TransactionTermsConditions.Where(x => x.ReferenceNumber == transactionEdit.aTransactionEdits.ReferenceNumber).ToList();


                    for (int index1 = 0; index1 < checker.Count; index1++)
                    {
                        bool isTrue = false;

                        for (int index2 = 0; index2 < transactionTerms.Count; index2++)
                        {
                            if (checker[index1].TermsAndConditionCode == transactionTerms[index2].TermsAndConditionCode)
                            {
                                checker[index1].Parameter1 = transactionTerms[index2].Parameter1;
                                checker[index1].Parameter2 = transactionTerms[index2].Parameter2;
                                checker[index1].ModifiedByUserId = userId;
                                checker[index1].ModifiedDate = DateTime.Now;
                                transactionTerms.RemoveAt(index2);
                                isTrue = true;
                            }

                        }

                        if (isTrue == false)
                        {
                            _context.TransactionTermsConditions.Remove(checker[index1]);

                        }

                    }
                    if (transactionTerms.Count != 0)
                    {
                        foreach (aTransactionTerms data in transactionTerms)
                        {
                            _context.TransactionTermsConditions.Add(new TransactionTermsCondition
                            {
                                ReferenceNumber = transactionEdit.aTransactionEdits.ReferenceNumber,
                                TermsAndConditionCode = data.TermsAndConditionCode,
                                TermsAndConditionDescription = data.TermsAndConditionDescription,
                                Parameter1 = data.Parameter1,
                                Parameter2 = data.Parameter2,
                                CreatedByUserId = transactionEdit.aTransactionEdits.CreatedByUserId,
                                CreatedDate = DateTime.Now,

                            });

                        }
                    }

                }
                else
                {
                    List<TransactionTermsCondition> checker = _context.TransactionTermsConditions.Where(x => x.ReferenceNumber == transactionEdit.aTransactionEdits.ReferenceNumber).ToList();
                    if (checker.Count != 0)
                    {
                        foreach (TransactionTermsCondition data in checker)
                        {
                            _context.TransactionTermsConditions.Remove(data);
                        }
                    }

                }

                _context.SaveChanges();

                return Ok();
            }
            catch (Exception e)
            {
                if (e.Message.Contains("InnerException") || e.Message.Contains("inner exception"))
                {

                    return StatusCode(202, "InnerExeption: " + e.InnerException);
                }
                else
                {

                    return StatusCode(202, "Error Message: " + e.Message);
                }
            }
        }

        [HttpPost("htmlToPdf")]
        public async Task<ActionResult> htmlToPdf(
           String referenceNumber,
           int userId
           )
        {
            try
            {

                Transaction transaction = _context.Transactions.Where(x => x.ReferenceNumber == referenceNumber).FirstOrDefault();

                Customer customer = _context.Customers.Where(x => x.IsDelete == false && x.Code == transaction.CustomerCode).FirstOrDefault();

                var user = _context.Users.Where(x => x.Id == userId).Select(x => new
                {
                    x.Fullname,
                    Position = _context.Positions.Where(i => i.Code == x.PositionCode).Select( s => s.Name ).FirstOrDefault()

                }).FirstOrDefault();

             

                List<TransactionDetail> transactionDetail = _context.TransactionDetails.Where(x => x.ReferenceNumber == referenceNumber).ToList();

                List<TransactionTermsCondition> transactionTermsConditions = _context.TransactionTermsConditions.Where(x => x.ReferenceNumber == referenceNumber).ToList();



                var currDir = Directory.GetCurrentDirectory();

                //FOR AUTO DOWNLOAD THE PDF W/ TECH RECOM
                var mjmlPath = Path.Combine(currDir, Path.Combine("prints", "transaction.html"));
                var content = System.IO.File.ReadAllText(mjmlPath);

                var splits = transaction.ReferenceDate.Split('/');

                var sad = DateTime.Parse(transaction.ReferenceDate).ToString("d MMMM yyyy");
                //var sad = DateTime.Parse(splits[1]+"/"+ splits[0] + "/" + splits[2]).ToString("d MMMM yyyy");

                // Replace placeholders in the HTML content with relevant data for downloading
                content = content.Replace("@reference", transaction.ReferenceNumber);
                content = content.Replace("@revision", transaction.RevisedCount.ToString());
                content = content.Replace("@refDate", sad);
                content = content.Replace("@position", customer.Position);
                content = content.Replace("@customerAddress", customer.CompanyAddress1);
                content = content.Replace("@company", customer.CompanyName);
                content = content.Replace("@customerName", transaction.CustomerName);

                var splitLastname = transaction.CustomerName.Split(" ");
                string lastname = "";

     

                foreach (string last in splitLastname)
                {
                    //lastname = last;

                    if(last != "")
                    {
                        lastname = last;
                    }
                    
               
                }

                string storageType = _context.Dropdowns.Where(x => x.Value == transaction.StorageType).Select(x=>x.Display).FirstOrDefault();

                content = content.Replace("@salutation", customer.Salutation);
                content = content.Replace("@lastname", lastname);

                content = content.Replace("@remarks", transaction.Remarks);
                content = content.Replace("@serviceAddress", transaction.ServiceAddress);
                content = content.Replace("@storageType", storageType);
                content = content.Replace("@commodity", transaction.Commodity);


                var notedBy = _context.Users.Where(x => x.Id == int.Parse(transaction.NotedByUserId.ToString())).Select(x => new
                {
                    x.Fullname,
                    Position = _context.Positions.Where(i => i.Code == x.PositionCode).Select( s => s.Name).FirstOrDefault()
                }).FirstOrDefault();


                content = content.Replace("@notedBy", notedBy.Fullname);
                content = content.Replace("@notedPos", notedBy.Position);
                content = content.Replace("@userName", user.Fullname);
                content = content.Replace("@userPos", user.Position);

                List<string> toChangeTransactionDetails = new List<string>();

                foreach (TransactionDetail data in transactionDetail)
                {

                    Rate rates = _context.Rates.Where(x => x.ChargeCode == data.ChargeCode && x.Status == 1).FirstOrDefault();

                    if (data.ChargeCode == "101")
                    {
                        toChangeTransactionDetails.Add("<tr style=\"color:black\"> " + "<td style=\"font-weight:bold\">" + rates.ChargeCategory + "<br>" + "<span style=\"color: cornflowerblue;\">" + storageType + "<span>" + "</td>" + "</td>" + "<td>" + data.Currency + "</td>" + "<td>" + data.Rates + "</td>" + "<td>" + data.Uom + "</td>" + "<td>" + data.Remarks + "</td>" + "</tr>");

                    }
                    else
                    {
                        toChangeTransactionDetails.Add("<tr style=\"color:black\"> " + "<td style=\"font-weight:bold\">" + rates.ChargeCategory + "</td>" + "</td>" + "<td>" + data.Currency + "</td>" + "<td>" + data.Rates + "</td>" + "<td>" + data.Uom + "</td>" + "<td>" + data.Remarks + "</td>" + "</tr>");

                    }

                       }

                content = content.Replace("@transactionDetails", String.Concat(toChangeTransactionDetails) );

                List<string> toChangeTransactionTermsCondition = new List<string>();

                int numbering = 1;
                foreach (TransactionTermsCondition data in transactionTermsConditions)
                {

                    string newDesc = "";

                    foreach (string desc in data.TermsAndConditionDescription.Split(" "))
                    {
                        if (desc.ToLower().Contains("parameter1"))
                        {
                            newDesc = $" {newDesc}\u00A0{data.Parameter1} ";
                        }
                        else if (desc.ToLower().Contains("parameter2"))
                        {
                            newDesc = $" {newDesc}\u00A0{data.Parameter2} ";
                        }
                        else
                        {
                            newDesc = $"{newDesc} {desc} ";
                        }
                    }
                    toChangeTransactionTermsCondition.Add("<p style=\"white-space: pre-wrap; Margin: 0; -webkit-text-size-adjust: none; -ms-text-size-adjust: none; mso-line-height-rule: exactly; font-family: arial, 'helvetica neue', helvetica, sans-serif; line-height: 21px; color: #333333; font-size: 14px \">" + numbering + ". " + newDesc+"</p>");
                    numbering++;
                   // toChangeTransactionTermsCondition.Add("<tr style=\"color:black\">" + "<td>" + data.TermsAndConditionCode + "</td>" + "<td>" + newDesc + "</td>" + "</tr>");
                }

              /*  for (int x = 0; x < 200; x++)
                {
                    toChangeTransactionTermsCondition.Add("<tr>" + "<td>" + "  data.TermsAndConditionCod " + "</td>" + "<td>" + "  data.TermsAndConditionCod " + "</td>" + "</tr>");
                }*/

                content = content.Replace("@termsAndConditionCode", String.Concat(toChangeTransactionTermsCondition));


                /*var converter = new HtmlConverter();


                 var bytes = converter.FromHtmlString(content);*/
               var tempFooter = Path.Combine(currDir, Path.Combine("prints", "footers.html"));
               var footer = System.IO.File.ReadAllText(tempFooter);

                var tempHeader = Path.Combine(currDir, Path.Combine("prints", "headers.html"));
                var header = System.IO.File.ReadAllText(tempHeader);

                var htmlToPdf = new NReco.PdfGenerator.HtmlToPdfConverter();
                htmlToPdf.Margins = new PageMargins { Top = 34,Bottom = 23,Left = 5,Right = 5};
                htmlToPdf.PageHeaderHtml = header;
                htmlToPdf.PageFooterHtml = footer;
                var pdfBytes = htmlToPdf.GeneratePdf(content);


                //var test = File(bytes, "application/pdf", "xxx.pdf");



                return File(pdfBytes, "application/pdf", "xxx.pdf");

            }
            catch (Exception e)
            {
                if (e.Message.Contains("InnerException") || e.Message.Contains("inner exception"))
                {

                    return StatusCode(202, "InnerExeption: " + e.InnerException);
                }
                else
                {

                    return StatusCode(202, "Error Message: " + e.Message);
                }
            }
        }

/*        public class PdfOptions
        {
            public string HtmlContent { get; set; }
            public string HeaderHtml { get; set; }
            public string FooterHtml { get; set; }
        }

        [HttpPost("newwww")]
        public async Task<ActionResult> newwww()
        {
            try {

                // Must have write permissions to the path folder
                PdfWriter writer = new PdfWriter("C:\\Users\\mjfalvarez\\Documents\\all gits folder\\demo.pdf");
                PdfDocument pdf = new PdfDocument(writer);
                Document document = new Document(pdf);
                Paragraph header = new Paragraph("HEADER")
                   .SetTextAlignment(TextAlignment.CENTER)
                   .SetFontSize(20);

                document.Add(header);
                document.Close();
                return Ok();
            }
            catch (Exception e)
            {
                if (e.Message.Contains("InnerException") || e.Message.Contains("inner exception"))
                {

                    return StatusCode(202, "InnerExeption: " + e.InnerException);
                }
                else
                {

                    return StatusCode(202, "Error Message: " + e.Message);
                }
            }
        }*/

       /* private static void AddHeader(PdfDocument doc, PdfMargins margin)
        {
            //Get the size of first page  
            SizeF pageSize = doc.Pages[0].Size;

            //Create a PdfPageTemplateElement object that will be  
            //use as header space  
            PdfPageTemplateElement headerSpace = new PdfPageTemplateElement(pageSize.Width, margin.Top);
            headerSpace.Foreground = true;
            doc.Template.Top = headerSpace;

            //Draw image at the top left of header space  
            PdfImage headerImage = PdfImage.FromFile(@"signature.png");
            float width = headerImage.Width / 7;
            float height = headerImage.Height / 7;
            headerSpace.Graphics.DrawImage(headerImage, 0, 0, width, height);

            //Draw text at the top right of header space  
            PdfTrueTypeFont font = new PdfTrueTypeFont(new Font("Arial", 9f, FontStyle.Bold | FontStyle.Italic), true);
            PdfStringFormat format = new PdfStringFormat(PdfTextAlignment.Right);
            String headerText = "\nAnnual Financial Report\n2017/03/24";
            float x = pageSize.Width;
            float y = 0;
            headerSpace.Graphics.DrawString(headerText, font, PdfBrushes.Black, x, y, format);
        }
        static void AddFooter(PdfDocument doc, PdfMargins margin)
        {
            //Get the size of first page  
            SizeF pageSize = doc.Pages[0].Size;

            //Create a PdfPageTemplateElement object that will be  
            //used as footer space  
            PdfPageTemplateElement footerSpace = new PdfPageTemplateElement(pageSize.Width, margin.Bottom);
            footerSpace.Foreground = true;
            doc.Template.Bottom = footerSpace;

            //Draw text at the center of footer space  
            PdfTrueTypeFont font = new PdfTrueTypeFont(new Font("Arial", 9f, FontStyle.Bold), true);
            PdfStringFormat format = new PdfStringFormat(PdfTextAlignment.Center);
            String headerText = "Copyright © 2017 xxx. All Rights Reserved.";
            float x = pageSize.Width / 2;
            float y = 15f;
            footerSpace.Graphics.DrawString(headerText, font, PdfBrushes.Black, x, y, format);

            //Create page number automatic field  
            PdfPageNumberField number = new PdfPageNumberField();
            //Create page count automatic field  
            PdfPageCountField count = new PdfPageCountField();
            //Add the fields in composite field  
            PdfCompositeField compositeField = new PdfCompositeField(font, PdfBrushes.Black, "Page {0} of {1}", number, count);
            //Align string of "Page {0} of {1}" to center   
            compositeField.StringFormat = new PdfStringFormat(PdfTextAlignment.Center, PdfVerticalAlignment.Middle);
            compositeField.Bounds = footerSpace.Bounds;
            //Draw composite field at footer space  
            compositeField.Draw(footerSpace.Graphics);
        }*/

    }

}
