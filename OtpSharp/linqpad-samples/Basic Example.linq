<Query Kind="Statements">
  <NuGetReference>OtpSharp</NuGetReference>
  <Namespace>OtpSharp</Namespace>
</Query>

var totp = (Totp)KeyUrl.FromUrl("otpauth://totp/test?secret=AEBAGBAFAYDQQCIAAEBAGBAFAYDQQCIAAEBAGBAFAYDQQCIA");
totp.ComputeTotp().Dump("Timed One Time Password");
