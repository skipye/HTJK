using DataProject;
using ModelProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServiceProject
{
    public class MessageService
    {
        private static readonly MessageDal MDal = new MessageDal();
        public bool AddMessage(MessageModel Models)
        {
            try { MDal.AddMessage(Models); return true; }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public bool AddReplyMessage(MessageModel Models)
        {
            try { MDal.AddReplyMessage(Models); return true; }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
            public List<MessageModel> GetMessagelist(int PageIndex, int PageSize, Guid? UserId)
        {
            try { return MDal.GetMessagelist(PageIndex, PageSize, UserId); }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
       
        
        public void UpdateMemberMessageState(Guid UserId)
        {
            try { MDal.UpdateMemberMessageState(UserId); }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public int? GetMessagelistConut(Guid? UserId)
        {
            try { return MDal.GetMessagelistConut(UserId); }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
