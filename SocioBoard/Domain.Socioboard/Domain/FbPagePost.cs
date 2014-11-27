﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Domain.Socioboard.Domain
{
  public class FbPagePost
    {
        public Guid Id { get; set; }        
        public Guid UserId { get; set; }
        public string PageId { get; set; }
        public string FromId { get; set; }
        public string FromName { get; set; }
        public string Type { get; set;}
        public string StatusType { get; set;}
        public string PictureUrl { get; set; }
        public string LinkUrl { get; set; }
        public string IconUrl { get; set; }
        public string PostId { get; set; }
        public string Post { get; set; }
        public DateTime PostDate { get; set; }
        public DateTime EntryDate { get; set; }
        public int Likes { get; set; }
        public int Comments { get; set; }
        public int Shares { get; set; }
        public int UserLikes { get; set; }
       
    }
}
