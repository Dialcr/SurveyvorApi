﻿namespace Services.Dtos;

public class UserIntputDto
{
    public string Username { get; set; } = "";
    public string Email { get; set; } = "";
    public string Password { get; set; } = "";
    //todo: quitar el role 
    //public string Role { get; set; } = "";
    public int? OrganizationId { get; set; }
}
