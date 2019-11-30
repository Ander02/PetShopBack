﻿namespace PetShop.Infrastructure.Notifications
{
    public enum NotificationType
    {
        Success,
        ResourceNotFound,
        InvalidArguments,
        BadRequest,
        UnauthenticatedAccess,
        ForbiddenAccess,
        ServerError
    }
}