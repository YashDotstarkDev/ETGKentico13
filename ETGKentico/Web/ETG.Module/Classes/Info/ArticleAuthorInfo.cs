using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

using CMS;
using CMS.DataEngine;
using CMS.Helpers;
using ETG.Module.Classes.Info;
using ETG.Module.Classes.Providers;

[assembly: RegisterObjectType(typeof(ArticleAuthorInfo), ArticleAuthorInfo.OBJECT_TYPE)]

namespace ETG.Module.Classes.Info
{
    /// <summary>
    /// Data container class for <see cref="ArticleAuthorInfo"/>.
    /// </summary>
    [Serializable]
    public partial class ArticleAuthorInfo : AbstractInfo<ArticleAuthorInfo>
    {
        /// <summary>
        /// Object type.
        /// </summary>
        public const string OBJECT_TYPE = "etg.articleauthor";


        /// <summary>
        /// Type information.
        /// </summary>
#warning "You will need to configure the type info."
        public static readonly ObjectTypeInfo TYPEINFO = new ObjectTypeInfo(typeof(ArticleAuthorInfoProvider), OBJECT_TYPE, "ETG.ArticleAuthor", "ArticleAuthorID", "ArticleAuthorLastModified", "ArticleAuthorGuid", "ArticleAuthorCodeName", "ArticleAuthorFullName", null, null, null, null)
        {
            ModuleName = "ETG",
            TouchCacheDependencies = true,
            ImportExportSettings =
            {
                IsExportable = true, // Makes the data of the custom Office class exportable
                AllowSingleExport = true, // Allows export of single office objects from the office listing page
                ObjectTreeLocations = new List<ObjectTreeLocation>()
                {
                    // Creates a new category in the global objects export interface
                    new ObjectTreeLocation(GLOBAL, "ArticleAuthorCodeName")
                }
            },
            SynchronizationSettings =
            {
                LogSynchronization = SynchronizationTypeEnum.LogSynchronization, // Enables logging of staging tasks for changes made to Office objects
                ObjectTreeLocations = new List<ObjectTreeLocation>()
                {
                    // Creates a new category in the 'Global objects' section of the staging object tree
                    new ObjectTreeLocation(GLOBAL, "ArticleAuthorCodeName")
                }
            }
        };


        /// <summary>
        /// Article author ID.
        /// </summary>
        [DatabaseField]
        public virtual int ArticleAuthorID
        {
            get
            {
                return ValidationHelper.GetInteger(GetValue("ArticleAuthorID"), 0);
            }
            set
            {
                SetValue("ArticleAuthorID", value);
            }
        }


        /// <summary>
        /// Article author full name.
        /// </summary>
        [DatabaseField]
        public virtual string ArticleAuthorFullName
        {
            get
            {
                return ValidationHelper.GetString(GetValue("ArticleAuthorFullName"), String.Empty);
            }
            set
            {
                SetValue("ArticleAuthorFullName", value);
            }
        }


        /// <summary>
        /// Article author code name.
        /// </summary>
        [DatabaseField]
        public virtual string ArticleAuthorCodeName
        {
            get
            {
                return ValidationHelper.GetString(GetValue("ArticleAuthorCodeName"), String.Empty);
            }
            set
            {
                SetValue("ArticleAuthorCodeName", value);
            }
        }


        /// <summary>
        /// Article author guid.
        /// </summary>
        [DatabaseField]
        public virtual Guid ArticleAuthorGuid
        {
            get
            {
                return ValidationHelper.GetGuid(GetValue("ArticleAuthorGuid"), Guid.Empty);
            }
            set
            {
                SetValue("ArticleAuthorGuid", value);
            }
        }


        /// <summary>
        /// Article author last modified.
        /// </summary>
        [DatabaseField]
        public virtual DateTime ArticleAuthorLastModified
        {
            get
            {
                return ValidationHelper.GetDateTime(GetValue("ArticleAuthorLastModified"), DateTimeHelper.ZERO_TIME);
            }
            set
            {
                SetValue("ArticleAuthorLastModified", value);
            }
        }


        /// <summary>
        /// Deletes the object using appropriate provider.
        /// </summary>
        protected override void DeleteObject()
        {
            ArticleAuthorInfoProvider.DeleteArticleAuthorInfo(this);
        }


        /// <summary>
        /// Updates the object using appropriate provider.
        /// </summary>
        protected override void SetObject()
        {
            ArticleAuthorInfoProvider.SetArticleAuthorInfo(this);
        }


        /// <summary>
        /// Constructor for de-serialization.
        /// </summary>
        /// <param name="info">Serialization info.</param>
        /// <param name="context">Streaming context.</param>
        protected ArticleAuthorInfo(SerializationInfo info, StreamingContext context)
            : base(info, context, TYPEINFO)
        {
        }


        /// <summary>
        /// Creates an empty instance of the <see cref="ArticleAuthorInfo"/> class.
        /// </summary>
        public ArticleAuthorInfo()
            : base(TYPEINFO)
        {
        }


        /// <summary>
        /// Creates a new instances of the <see cref="ArticleAuthorInfo"/> class from the given <see cref="DataRow"/>.
        /// </summary>
        /// <param name="dr">DataRow with the object data.</param>
        public ArticleAuthorInfo(DataRow dr)
            : base(TYPEINFO, dr)
        {
        }
    }
}