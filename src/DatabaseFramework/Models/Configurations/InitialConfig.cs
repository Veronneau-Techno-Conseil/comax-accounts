﻿using DatabaseFramework.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatabaseFramework.Models.Configurations
{
    /// <summary>
    /// Initial configuration for any extensions added to the default AspNet and openiddict authentication system
    /// </summary>
    public class InitialConfig : IModelConfig
    {
        public void SetupFields(ModelBuilder builder)
        {
            builder.Entity<User>()
                .Property(x => x.AccountTypeId)
                .IsRequired();

            //This was added to set composite keys for the ApplicationTypeMaps and UserApplicationMap
            builder.Entity<ApplicationTypeMap>()
                .HasKey(x => new { x.ApplicationId, x.ApplicationTypeId });
            builder.Entity<UserApplicationMap>()
                .HasKey(x => new { x.UserId, x.ApplicationId });

            builder.Entity<UserApplicationMap>()
               .HasKey(x => new { x.ApplicationId });
            builder.Entity<ApplicationTypeMap>()
                .HasKey(x => new { x.ApplicationId });

            builder.Entity<Contact>()
                .Property(x => x.PrimaryAccountId)
                .IsRequired();

            builder.Entity<Group>()
                .Property(x => x.OwnerId)
                .IsRequired();

            builder.Entity<GroupMember>()
                .Property(x => x.UserId)
                .IsRequired();

            builder.Entity<Notification>()
                .Property(x => x.ContactMethodTypeId)
                .IsRequired();

            builder.Entity<Notification>()
                .Property(x => x.ContactId)
                .IsRequired();

            builder.Entity<ContactRequest>()
                .Property(x => x.IsDeleted)
                .HasDefaultValue(false);

            builder.Entity<Contact>()
                .Property(x => x.IsDeleted)
                .HasDefaultValue(false);

            builder.Entity<Group>()
                .Property(x => x.IsDeleted)
                .HasDefaultValue(false);

        }

        public void SetupRelationships(ModelBuilder builder)
        {
            builder.Entity<User>()
                .HasOne(x => x.AccountType)
                .WithMany()
                .HasForeignKey(x => x.AccountTypeId);

            builder.Entity<ApplicationTypeMap>()
                .HasOne(x => x.ApplicationType)
                .WithMany()
                .HasForeignKey(x => x.ApplicationTypeId);

            builder.Entity<ApplicationTypeMap>()
                .HasOne(x => x.Application)
                .WithMany(x=>x.ApplicationTypeMaps)
                .HasForeignKey(x => x.ApplicationId);

            builder.Entity<UserApplicationMap>()
                .HasOne(x => x.User)
                .WithMany(x=>x.ApplicationMaps)
                .HasForeignKey(x => x.UserId);

            builder.Entity<UserApplicationMap>()
                .HasOne(x => x.Application)
                .WithMany(x=>x.UserApplicationMaps)
                .HasForeignKey(x => x.ApplicationId);

            builder.Entity<Contact>()
                .HasOne(x => x.PrimaryAccount)
                .WithMany()
                .HasForeignKey(x => x.PrimaryAccountId);

            builder.Entity<Contact>()
                .HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId);

            builder.Entity<Contact>()
                .HasOne(x => x.CreationStatus)
                .WithMany()
                .HasForeignKey(x => x.CreationStatusId);

            builder.Entity<ContactRequest>()
                .HasOne(x => x.Contact)
                .WithMany()
                .HasForeignKey(x => x.ContactId);

            builder.Entity<ContactRequest>()
                .HasOne(x => x.ContactStatus)
                .WithMany()
                .HasForeignKey(x => x.ContactStatusId);

            builder.Entity<ContactRequest>()
                .HasOne(x => x.Notification)
                .WithMany()
                .HasForeignKey(x => x.NotificationId);

            builder.Entity<ContactRequest>()
                .HasOne(x => x.IdProvider)
                .WithMany()
                .HasForeignKey(x => x.IdProviderId);

            builder.Entity<Group>()
                .HasOne(x => x.Owner)
                .WithMany()
                .HasForeignKey(x => x.OwnerId);

            builder.Entity<GroupMember>()
                .HasOne(x => x.Group)
                .WithMany()
                .HasForeignKey(x => x.GroupId);

            builder.Entity<GroupMember>()
                .HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId);

            builder.Entity<GroupMemberRole>()
                .HasOne(x => x.GroupMember)
                .WithMany()
                .HasForeignKey(x => x.GroupMemberId);

            builder.Entity<GroupMemberRole>()
                .HasOne(x => x.GroupRole)
                .WithMany()
                .HasForeignKey(x => x.GroupRoleId);

            builder.Entity<Notification>()
                .HasOne(x => x.Contact)
                .WithMany()
                .HasForeignKey(x => x.ContactId);

            builder.Entity<Notification>()
                .HasOne(x => x.ContactMethodType)
                .WithMany()
                .HasForeignKey(x => x.ContactMethodTypeId);
        }

        public void SetupTables(ModelBuilder builder)
        {
            builder.Entity<AccountType>()
                .ToTable("AccountTypes");

            builder.Entity<ApplicationType>()
                .ToTable("ApplicationTypes");

            builder.Entity<ApplicationTypeMap>()
                .ToTable("ApplicationTypeMaps");

            builder.Entity<UserApplicationMap>()
                .ToTable("UserApplicationMap");

            builder.Entity<CreationStatus>()
                .ToTable("CreationStatus");
            
            builder.Entity<IdProvider>()
                .ToTable("IdProvider");
            
            builder.Entity<ContactMethodType>()
                .ToTable("ContactMethodType");
            
            builder.Entity<Contact>()
                .ToTable("Contacts");

            builder.Entity<Notification>()
                .ToTable("Notifications");

            builder.Entity<ContactRequest>()
                .ToTable("ContactRequests");

            builder.Entity<GroupRole>()
                .ToTable("GroupRole");

            builder.Entity<Group>()
                .ToTable("Groups");

            builder.Entity<GroupMember>()
                .ToTable("GroupMembers");

            builder.Entity<GroupMemberRole>()
                .ToTable("GroupMemberRole");

        }
    }
}
