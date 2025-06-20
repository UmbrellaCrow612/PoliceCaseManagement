﻿using Cases.Core.Models;
using Cases.Core.Models.Joins;
using Cases.Core.ValueObjects;
using Results.Abstractions;

namespace Cases.Core.Services
{
    /// <summary>
    /// Represents the business contract to interact with cases and there dependences
    /// </summary>
    public interface ICaseService
    {
        /// <summary>
        /// Create a case in the system.
        /// </summary>
        Task<CaseResult> CreateAsync(Case caseToCreate);

        /// <summary>
        /// Create a IncidentType that cases can link to.
        /// </summary>
        Task<CaseResult> CreateIncidentType(IncidentType incidentType);

        /// <summary>
        /// Link a <see cref="Case"/> to a <see cref="IncidentType"/> through the join table <see cref="Cases.Core.Models.Joins.CaseIncidentType"/>
        /// </summary>
        /// <returns></returns>
        Task<CaseResult> AddToIncidentType(Case @case, IncidentType incidentType);

        /// <summary>
        /// Adds a <see cref="CaseAction"/> to a <see cref="Case"/> case actions have a one to many relation to cases only one action belongs to one case
        /// </summary>
        /// <param name="case">The case you want to add a case action to</param>
        /// <param name="caseAction">The action you want to record</param>
        Task<CaseResult> AddCaseAction(Case @case, CaseAction caseAction);

        /// <summary>
        /// Find a case by it's <see cref="Case.Id"/>
        /// </summary>
        /// <param name="caseId"></param>
        /// <returns></returns>
        Task<Case?> FindById(string caseId);

        /// <summary>
        /// Find a incident type by it's ID.
        /// </summary>
        Task<IncidentType?> FindIncidentTypeById(string incidentTypeId);

        /// <summary>
        /// Get all <see cref="IncidentType"/> defined in the system that a <see cref="Case"/> can link to.
        /// </summary>
        /// <returns></returns>
        Task<List<IncidentType>> GetAllIncidentTypes();

        /// <summary>
        /// Get the number of times a <see cref="IncidentType"/> is linked to x many cases through the join table <see cref="Models.Joins.CaseIncidentType"/>
        /// </summary>
        /// <param name="incidentType">The incident type you want to know how many cases it is linked to.</param>
        /// <returns></returns>
        Task<int> GetCaseIncidentCount(IncidentType incidentType);

        /// <summary>
        /// Delete a incident type - unlinks it from any cases it's linked to and then deleted.
        /// </summary>
        Task<CaseResult> DeleteIncidentType(IncidentType incidentType);

        /// <summary>
        /// Update a incident type.
        /// </summary>
        Task<CaseResult> UpdateIncidentType(IncidentType incidentType);

        /// <summary>
        /// Checks if a <see cref="Case.CaseNumber"/> is taken by another case
        /// </summary>
        /// <param name="caseNumber">The case number</param>
        /// <returns>Flag to indicate if a case number is taken</returns>
        Task<bool> IsCaseNumberTaken(string caseNumber);

        /// <summary>
        /// Search for cases in the system with a query object
        /// </summary>
        /// <param name="query">The query object</param>
        /// <returns>Pagination result of cases</returns>
        Task<PagedResult<Case>> SearchCases(SearchCasesQuery query);

        /// <summary>
        /// Get <see cref="IncidentType"/> linked to the <see cref="Case"/> passed in through the join table <see cref="Models.Joins.CaseIncidentType"/>
        /// </summary>
        /// <param name="case">The case you want to the get the linked incident types to it</param>
        /// <returns>List of incident types</returns>
        Task<List<IncidentType>> GetIncidentTypes(Case @case);

        /// <summary>
        /// Update a cases linked incident types through the join table case incident types
        /// it removes all currently linked incident types and then links the passed incident types to it
        /// </summary>
        /// <param name="case">The case you want to update</param>
        /// <param name="incidentTypes">List of new incident types you want to link to it</param>
        Task<CaseResult> UpdateCaseLinkedIncidentTypes(Case @case, List<IncidentType> incidentTypes);

        /// <summary>
        /// Get all case actions for a given case
        /// </summary>
        /// <param name="case">The case you want to get all case actions for</param>
        Task<List<CaseAction>> GetCaseActions(Case @case);

        /// <summary>
        /// Get all users linked to a case
        /// </summary>
        /// <param name="case">Case you want to get info for</param>
        Task<List<CaseUser>> GetCaseUsers(Case @case);

        /// <summary>
        /// Assign a set of users to a case, gives them default permissions for the case
        /// </summary>
        /// <param name="case">The case to link to</param>
        /// <param name="userIds">A set of user id's to link to it</param>
        Task<CaseResult> AddUsers(Case @case, List<string> userIds);

        /// <summary>
        /// Remove assigned user from a case
        /// </summary>
        /// <param name="userId">The ID of he user to remove</param>
        /// <param name="case">The case you want to remove them from</param>
        Task<CaseResult> RemoveUser(Case @case, string userId);

        /// <summary>
        /// Add a file to a case as a Attachment
        /// </summary>
        /// <param name="case">The case to add it to</param>
        /// <param name="metaData">The file to upload meta data</param>
        /// <returns>Pre signed URL used to upload the file and the ID of the filet</returns>
        Task<(string preSignedUrl, string fileId)> AddAttachment(Case @case, UploadCaseAttachmentFileMetaData metaData);

        /// <summary>
        /// Get all <see cref="CaseAttachmentFile"/> linked to the given case
        /// </summary>
        /// <param name="case">The case to get the files for</param>
        /// <returns>List of files</returns>
        Task<List<CaseAttachmentFile>> GetCaseAttachments(Case @case);

        /// <summary>
        /// Find a specific <see cref="CaseAttachmentFile"/> by it's ID
        /// </summary>
        /// <param name="caseAttachmentId">The ID of <see cref="CaseAttachmentFile.Id"/></param>
        Task<CaseAttachmentFile?> FindCaseAttachmentById(string caseAttachmentId);

        /// <summary>
        /// Download a <see cref="CaseAttachmentFile"/> as a a download URL for the client
        /// </summary>
        /// <param name="caseAttachmentFile">The file to download</param>
        /// <returns>Download URL</returns>
        Task<string> DownloadCaseAttachment(CaseAttachmentFile caseAttachmentFile);

        /// <summary>
        /// Soft delete's a specific <see cref="CaseAttachmentFile"/>
        /// </summary>
        /// <param name="file">The specific attachment to delete</param>
        Task<CaseResult> DeleteAttachment(CaseAttachmentFile file);


        /// <summary>
        /// Quick look up to check if a user has been assigned to to the given case <see cref="Case.CaseUsers"/> meaning that they can see it's details, this dose not check
        /// there roles but only that they are linked
        /// </summary>
        /// <param name="caseId">The case ID to check</param>
        /// <param name="userId">The user ID to check</param>
        /// <returns>Result object, if it is <see cref="CaseResult.Succeeded"/> as true it means they can view it else they cannot</returns>
        Task<CaseResult> CanUserViewCaseDetails(string caseId, string userId);

        /// <summary>
        /// Get a list of <see cref="CasePermission"/> set on the given case
        /// </summary>
        /// <param name="case">The case to get the permissions for</param>
        Task<List<CasePermission>> GetCasePermissions(Case @case);

        /// <summary>
        /// Find a <see cref="CasePermission"/> by it's ID
        /// </summary>
        /// <param name="permissionId">The case permission to find</param>
        /// <param name="case">The parent case it is linked to</param>
        Task<CasePermission?> FindCasePermissionById(Case @case, string permissionId);

        /// <summary>
        /// Update a <see cref="CasePermission"/>
        /// </summary>
        /// <param name="permission">The permission to update</param>
        Task<CaseResult> UpdateCasePermission(CasePermission permission);

        /// <summary>
        /// Get a users case permission on a given case
        /// </summary>
        /// <param name="case">The case to get there permission for</param>
        /// <param name="userId">The user who's permission you want to get</param>
        /// <returns>List of permissions they have if the operation succeeds</returns>
        Task<MyCasePermissionResult> GetUserCasePermissions(Case @case, string userId);

        /// <summary>
        /// Checks if a user has <see cref="CasePermission.CanViewPermissions"/> flag set to true for the given case
        /// </summary>
        /// <param name="caseId">The case to check it for</param>
        /// <param name="userId">The ID of the user</param>
        /// <returns>Result if it is <see cref="CaseResult.Succeeded"/> then they do have permissions to view them</returns>
        Task<CaseResult> CanUserViewCasePermissions(string caseId, string userId);

        /// <summary>
        /// Checks if a user has <see cref="CasePermission.CanEditPermissions"/> flag set to true
        /// </summary>
        /// <param name="caseId">The case to check</param>
        /// <param name="userId">The user to check for</param>
        /// <returns><see cref="CaseResult.Succeeded"/> if they can or cannot</returns>
        Task<CaseResult> CanUserEditCasePermissions(string caseId, string userId);

        /// <summary>
        /// Checks to see if the given user has the permissions to view the actions taken on a case
        /// </summary>
        /// <param name="caseId">The ID of the case to check</param>
        /// <param name="userId">The ID of the user to check</param>
        /// <returns><see cref="CaseResult.Succeeded"/> if they can or not</returns>
        Task<CaseResult> CanUserViewCaseActions(string caseId, string userId);

        /// <summary>
        /// Checks if a user can create actions on a given case
        /// </summary>
        /// <param name="caseId">The case to check</param>
        /// <param name="userId">The user to check</param>
        /// <returns><see cref="CaseResult.Succeeded"/> if they can or cannot</returns>
        Task<CaseResult> CanUserCreateCaseActions(string caseId, string userId);

        /// <summary>
        /// Checks if a user can edit linked incident types to a given case NOTE it dose not mean they can edit the incident type itself but a 
        /// specific cases linked incident types i.e ones it it linked to
        /// </summary>
        /// <param name="caseId">The case to check for</param>
        /// <param name="userId">The user to check for</param>
        /// <returns><see cref="CaseResult.Succeeded"/> if they can or cannot</returns>
        Task<CaseResult> CanUserEditLinkedIncidentTypes(string caseId, string userId);

        /// <summary>
        /// Update a <see cref="CaseAttachmentFile"/> in the system
        /// </summary>
        /// <param name="caseAttachmentFile">The file to update with new properties</param>
        Task<CaseResult> UpdateCaseAttachmentFile(CaseAttachmentFile caseAttachmentFile);
    }

    /// <summary>
    /// Standard result object used in a case service function
    /// </summary>
    public class CaseResult : IResult
    {
        public bool Succeeded { get; set; } = false;
        public ICollection<IResultError> Errors { get; set; } = [];

        public void AddError(string code, string? message = null)
        {
            Errors.Add(new CaseError { Code = code, Message = message });
        }
    }

    /// <summary>
    /// Standard case service error shape
    /// </summary>
    public class CaseError : IResultError
    {
        public required string Code { get; set; }
        public string? Message { get; set; }
    }

    public class MyCasePermissionResult : CaseResult
    {
        public List<string> Permissions { get; set; } = [];
    }
}
