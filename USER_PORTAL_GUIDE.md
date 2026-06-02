# User Portal Guide

## Overview

The ThreadifyLab website now includes a complete user portal with registration, login, and email verification functionality.

## Features

### User Registration
- Users can create accounts with:
  - First Name and Last Name
  - Email Address (must be unique)
  - Password (minimum 6 characters)
  - Optional Phone Number
- Automatic email verification token generation
- Verification email sent upon registration

### Email Verification
- Users receive a verification email after registration
- Email contains a secure token link
- Tokens expire after 24 hours
- Users can resend verification emails from the login page
- Users must verify email before logging in

### User Login
- Email and password authentication
- Secure cookie-based sessions
- 8-hour session expiration with sliding renewal
- Redirects to dashboard after successful login

### User Dashboard
- Personalized welcome page
- Account information display
- Quick access to future features (orders, settings, support)
- Secure logout functionality

## Database Schema

The `Users` table includes:
- User identification (Id, FirstName, LastName, Email)
- Authentication (PasswordHash)
- Email verification (IsEmailVerified, EmailVerificationToken, EmailVerificationTokenExpiry)
- Account status (IsActive, CreatedAt, LastLoginAt)
- Optional contact (Phone)

## Email Configuration

To enable email sending, configure `appsettings.json`:

```json
{
  "EmailSettings": {
    "SmtpHost": "smtp.gmail.com",
    "SmtpPort": "587",
    "SmtpUser": "your-email@gmail.com",
    "SmtpPassword": "your-app-password",
    "FromEmail": "noreply@threadifylab.com",
    "FromName": "ThreadifyLab"
  },
  "AppSettings": {
    "BaseUrl": "https://threadifylab.com"
  }
}
```

### Gmail Setup
1. Enable 2-Factor Authentication on your Google account
2. Generate an App Password: https://myaccount.google.com/apppasswords
3. Use the app password (not your regular password) in `SmtpPassword`

### Other Email Providers
- **Outlook/Hotmail**: `smtp-mail.outlook.com`, Port 587
- **Yahoo**: `smtp.mail.yahoo.com`, Port 587
- **Custom SMTP**: Use your provider's SMTP settings

## User Flow

### Registration Flow
1. User visits `/User/Register`
2. Fills out registration form
3. System creates account with unverified status
4. Verification email sent automatically
5. User redirected to success page with instructions

### Verification Flow
1. User clicks verification link in email
2. System validates token and expiry
3. Email marked as verified
4. User can now log in

### Login Flow
1. User visits `/User/Login`
2. Enters email and password
3. System validates credentials and email verification status
4. If valid, creates authenticated session
5. User redirected to dashboard

## Security Features

- **Password Hashing**: SHA256 with Base64 encoding
- **Email Verification**: Required before login
- **Token Expiry**: Verification tokens expire after 24 hours
- **Secure Tokens**: Cryptographically random token generation
- **Session Management**: Cookie-based with secure settings
- **Account Status**: Active/inactive account control

## Navigation Updates

The main navigation now shows:
- **Logged Out**: "Login" and "Sign Up" buttons
- **Logged In**: User's name and "Logout" button

## API Endpoints

### Public Endpoints
- `GET /User/Register` - Registration form
- `POST /User/Register` - Process registration
- `GET /User/Login` - Login form
- `POST /User/Login` - Process login
- `GET /User/VerifyEmail?token=xxx` - Verify email
- `POST /User/ResendVerification` - Resend verification email

### Protected Endpoints
- `GET /User/Dashboard` - User dashboard (requires authentication)
- `POST /User/Logout` - Logout (requires authentication)

## Testing Email Functionality

### Development Mode
If email settings are not configured, the system will:
- Still create user accounts
- Log a warning about missing email configuration
- Allow manual email verification via database update

### Manual Verification (Development)
```sql
UPDATE Users SET IsEmailVerified = 1 WHERE Email = 'user@example.com';
```

## Troubleshooting

### Email Not Sending
1. Check `appsettings.json` email settings
2. Verify SMTP credentials
3. Check firewall/network restrictions
4. Review application logs for errors

### Verification Token Expired
- User can request a new verification email from login page
- Or admin can manually verify in database

### Can't Login After Registration
- Verify email was sent and clicked
- Check `IsEmailVerified` field in database
- Ensure account is active (`IsActive = 1`)

### Password Issues
- Passwords are hashed and cannot be recovered
- Password reset feature coming soon
- Admin can update password hash in database if needed

## Future Enhancements

- Password reset functionality
- Email change verification
- Profile management
- Order history
- Account settings
- Two-factor authentication

## Database Migration

Run the updated schema to add the Users table:

```bash
mysql -u root -p threadifylabdb < Database/schema.sql
```

Or manually add the Users table using the SQL in `Database/schema.sql`.












