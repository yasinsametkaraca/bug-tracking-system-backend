using Microsoft.EntityFrameworkCore;
using Ticket.Core.Common;
using Ticket.Data;
using Ticket.Models;

namespace Ticket.Service
{
    public class UserRequestService
    {
        private readonly TicketDbContext _ticketDbContext;
        public UserRequestService(TicketDbContext ticketDbContext)
        {
            _ticketDbContext = ticketDbContext;
        }

        public async Task<Result<List<UserRequestSummary>>> GetUserRequestSummary()
        {
            //var vResult = await (from rq in _ticketDbContext.Requests
 
            //         .OrderBy(rq => rq.CreateDate)
            //         .ThenBy(rq => rq.Status)
            //         join jp in _ticketDbContext.Projects on rq.ProjectId equals jp.Id
            //         //join js in _ticketDbContext.Projects on rq.ModuleId equals js.Id
            //         //join jb in _ticketDbContext.Customers on rq.CustomerId equals jb.Id
            //         // join ja in _ticketDbContext.Users on rq.UserId equals ja.Id

            var vResult = await (from rq in _ticketDbContext.Requests
                        .Join(
                        _ticketDbContext.Projects,
                        rq => rq.ProjectId,
                        p => p.Id,
                        (rq, p) => new {
                                Id = rq.Id,
                                Definition = p.Definition,
                                ModuleId = rq.ModuleId,
                                CustomerId = rq.CustomerId,
                                UserId = rq.UserId,
                                ErrorDescription = rq.ErrorDescription,
                                CreateDate = rq.CreateDate,
                                UpdateDate = rq.UpdateDate,
                                ErrorTitle = rq.ErrorTitle,
                                Status = rq.Status,
                                ProjectId = rq.ProjectId
                            
                            }
                        )
                          .Join(
                        _ticketDbContext.Projects,
                        rq => rq.ModuleId,
                        p => p.Id,
                        (rq, p) => new {
                            Id = rq.Id,
                            Definition = rq.Definition,
                            ModuleDefinition = p.Definition,
                            CustomerId = rq.CustomerId,
                            UserId = rq.UserId,
                            ErrorDescription = rq.ErrorDescription,
                            CreateDate = rq.CreateDate,
                            UpdateDate = rq.UpdateDate,
                            ErrorTitle = rq.ErrorTitle,
                            Status = rq.Status,
                            ProjectId = rq.ProjectId,
                            ModuleId = rq.ModuleId
                          
                        }
                        )
                          .Join(
                        _ticketDbContext.Customers,
                        rq => rq.CustomerId,
                        c => c.Id,
                        (rq, c) => new {
                            Id = rq.Id,
                            Definition = rq.Definition,
                            ModuleDefinition = rq.ModuleDefinition,
                            Name = c.Name,
                            UserId = rq.UserId,
                            ErrorDescription = rq.ErrorDescription,
                            CreateDate = rq.CreateDate,
                            UpdateDate = rq.UpdateDate,
                            ErrorTitle = rq.ErrorTitle,
                            Status = rq.Status,
                            ProjectId = rq.ProjectId,
                            ModuleId = rq.ModuleId,
                            CustomerId = rq.CustomerId
                           
                        }
                        )
                         .Join(
                        _ticketDbContext.Users,
                        rq => rq.UserId,
                        u => u.Id,
                        (rq, u) => new {
                            Id = rq.Id,
                            Definition = rq.Definition,
                            ModuleDefinition = rq.ModuleDefinition,
                            Name = rq.Name,
                            UserNameSurname = u.UserNameSurname,
                            ErrorDescription = rq.ErrorDescription,
                            CreateDate = rq.CreateDate,
                            UpdateDate = rq.UpdateDate,
                            ErrorTitle = rq.ErrorTitle,
                            Status = rq.Status,
                            ProjectId = rq.ProjectId,
                            ModuleId = rq.ModuleId,
                            CustomerId = rq.CustomerId,
                            UserId = rq.UserId
                        }
                        ).OrderBy(rq => rq.CreateDate)
                         .ThenBy(rq => rq.Status)


                                 select new UserRequestSummary()

                                 {
                                    Id = rq.Id,
                                    Definition = rq.Definition,
                                    ModuleDefinition = rq.ModuleDefinition,
                                    ErrorDescription = rq.ErrorDescription,
                                    Name = rq.Name,
                                    CreateDate = rq.CreateDate,
                                    UpdateDate = rq.UpdateDate,
                                    ErrorTitle = rq.ErrorTitle,
                                    UserNameSurname = rq.UserNameSurname,
                                    Status = rq.Status,
                                    ProjectId=rq.ProjectId,
                                    ModuleId=rq.ModuleId,
                                    CustomerId=rq.CustomerId,
                                    UserId=rq.UserId


                                    //Definition = jp.Definition,
                                    //ModuleDefinition = js.Definition,
                                    //Name = jb.Name,
                                    //UserNameSurname = ja.UserNameSurname

                                 })
                                .ToListAsync();

            return Result<List<UserRequestSummary>>.PrepareSuccess(vResult);
        }

        public async Task<Result<UserRequestInfo>> GetUserRequestInfo(int requestId)
        {
                try
                {
                var vUser = await _ticketDbContext.Requests
                .Select(cm => new UserRequestInfo()

                {
                    Id = cm.Id,
                    ErrorDescription=cm.ErrorDescription,
                    CreateDate = cm.CreateDate,
                    UpdateDate = cm.UpdateDate,
                    Status = cm.Status,
                    UserId = cm.UserId, 
                    ProjectId = cm.ProjectId,
                    ModuleId=cm.ModuleId,   
                    CustomerId=cm.CustomerId,   
                    ErrorTitle=cm.ErrorTitle,   
                
                }).FirstOrDefaultAsync();

                return Result<UserRequestInfo>.PrepareSuccess(vUser);
                }
            catch (Exception e)
                {
                return Result<UserRequestInfo>.PrepareFailure("Hata mesajı");
                }
        }

        public async Task<Result<List<UserRequestInfo>>> GetUserRequestByCustomer(int customerId)
        {
            var vCustomer = _ticketDbContext.Requests.FirstOrDefault(cm => cm.Id == customerId);
            var vCustomerRequest = await (from rs in _ticketDbContext.Requests
            

                               .OrderBy(rs => rs.CreateDate)
                               .ThenBy(rs => rs.Status)
                               .ThenBy(rs => rs.CustomerId)
                               

                                join jb in _ticketDbContext.Projects on rs.ProjectId equals jb.Id
                                join js in _ticketDbContext.Projects on rs.ModuleId equals js.Id
                                join jc in _ticketDbContext.Users on rs.UserId equals jc.Id
                                join ja in _ticketDbContext.Customers on rs.CustomerId equals ja.Id

                                select new UserRequestInfo()
                                {
                                    Id = rs.Id,
                                    CustomerId = rs.CustomerId,
                                    UserId = rs.UserId,
                                    ProjectId = rs.ProjectId,
                                    CreateDate = rs.CreateDate,
                                    ModuleId = rs.ModuleId,
                                    ErrorDescription = rs.ErrorDescription,
                                    ErrorTitle = rs.ErrorTitle,
                                    UpdateDate = rs.UpdateDate,
                                    Status = rs.Status,
                                    Definition = jb.Definition,
                                    ModuleDefinition = js.Definition,
                                    UserNameSurname = jc.UserNameSurname,
                                    Name = ja.Name
                                   
                                })
                                  .ToListAsync();

            return Result<List<UserRequestInfo>>.PrepareSuccess(vCustomerRequest);
        }
        

        //public async Task<Result<List<UserRequestSummary>>> GetUserRequestByCustomerSummary(int customerId)
        //{
        //    try
        //    {
        //        var vCustomerRequest = await _ticketDbContext.Requests.Where(cm => cm.CustomerId == customerId)
        //            .Select(cm => new UserRequestSummary()

        //            {
        //                Id = cm.Id,
        //                ProjectId = cm.ProjectId,
        //                ModuleId = cm.ModuleId,
        //                CustomerId = cm.CustomerId,
        //                UserId = cm.UserId,
        //                ErrorDescription = cm.ErrorDescription,
        //                CreateDate = cm.CreateDate,
        //                UpdateDate = cm.UpdateDate,
        //                ErrorTitle = cm.ErrorTitle,
        //                Status = cm.Status,

        //            })
        //            .ToListAsync();

        //        if (vCustomerRequest == null)
        //            return Result<List<UserRequestSummary>>.PrepareFailure("Kayıt yok");

        //        return Result<List<UserRequestSummary>>.PrepareSuccess(vCustomerRequest);
        //    }
        //    catch (Exception e)
        //    {
        //        return Result<List<UserRequestSummary>>.PrepareFailure(e.Message);
        //    }
        //}

        public async Task<Result<List<UserRequestSummary>>> GetUserRequestSummary(int userId)
         {
            try
            {
                var vUser = _ticketDbContext.Users.FirstOrDefault(cm => cm.Id == userId);

                var vUserRequest = await _ticketDbContext.Requests
                    .Where(cm => (vUser.Role == "Admin" || cm.UserId == userId))
                    .Select(cm => new UserRequestSummary()
                    {
                        Id = cm.Id,
                        ProjectId = cm.ProjectId,
                        ModuleId = cm.ModuleId,
                        CustomerId = cm.CustomerId,
                        UserId = cm.UserId,
                        ErrorDescription = cm.ErrorDescription,
                        CreateDate = cm.CreateDate,
                        UpdateDate = cm.UpdateDate,
                        ErrorTitle = cm.ErrorTitle,
                        Status = cm.Status,
                      
                    })
                    .ToListAsync();

                return Result<List<UserRequestSummary>>.PrepareSuccess(vUserRequest);
            }
            catch (Exception e)
            {

                return Result<List<UserRequestSummary>>.PrepareFailure(e.Message);
            }
        }

        public async Task<Result> AddUserRequest(UserRequestInfo userRequestInfo, int userId)
        {
            var vUser = _ticketDbContext.Users.FirstOrDefault(cm => cm.Id == userId);

            if (vUser.Role != "Admin")
            {
                return Result.PrepareFailure("");
            }

            var vAddRequest = await _ticketDbContext.Requests.Where(x => x.Id == userRequestInfo.Id).FirstOrDefaultAsync();
            if (vAddRequest == null)
            {
                return Result.PrepareFailure("Tüm değerleri doldurmanız gerekiyor!");
            }

            _ticketDbContext.Requests.Attach(vAddRequest);

            vAddRequest.Id = userRequestInfo.Id;
            vAddRequest.ProjectId = userRequestInfo.ProjectId;
            vAddRequest.ModuleId = userRequestInfo.ModuleId;
            vAddRequest.CustomerId = userRequestInfo.CustomerId;
            vAddRequest.CreateDate = userRequestInfo.CreateDate;
            vAddRequest.UserId = userRequestInfo.UserId;
            vAddRequest.UpdateDate = DateTime.Now;
            vAddRequest.Status = userRequestInfo.Status;
            vAddRequest.ErrorTitle = userRequestInfo.ErrorTitle;
            vAddRequest.ErrorDescription = userRequestInfo.ErrorDescription;

            await _ticketDbContext.SaveChangesAsync();

            return Result.PrepareSuccess();
        }

        public async Task<Result> UpdateUserRequest(UserRequestInfo userRequestInfo, int userId)
        {
            var vUser = _ticketDbContext.Users.FirstOrDefault(cm => cm.Id == userId);

            if (vUser.Role != "Admin")
            {
                return Result.PrepareFailure("");
            }

            var vUpdateRequest = await _ticketDbContext.Requests.Where(x => x.Id == userRequestInfo.Id).FirstOrDefaultAsync();
            if (vUpdateRequest==null)
            {
                return Result.PrepareFailure("Tüm değerleri doldurmanız gerekiyor!");
            }

            _ticketDbContext.Requests.Attach(vUpdateRequest);

            vUpdateRequest.Id = userRequestInfo.Id;
            vUpdateRequest.ProjectId = userRequestInfo.ProjectId;
            vUpdateRequest.ModuleId = userRequestInfo.ModuleId;
            vUpdateRequest.CustomerId = userRequestInfo.CustomerId;
            vUpdateRequest.CreateDate = userRequestInfo.CreateDate;
            vUpdateRequest.UserId = userRequestInfo.UserId;
            vUpdateRequest.UpdateDate = DateTime.Now;
            vUpdateRequest.Status = userRequestInfo.Status;
            vUpdateRequest.ErrorTitle = userRequestInfo.ErrorTitle;
            vUpdateRequest.ErrorDescription = userRequestInfo.ErrorDescription;

            await _ticketDbContext.SaveChangesAsync();

            return Result.PrepareSuccess();
        }
    }
}
