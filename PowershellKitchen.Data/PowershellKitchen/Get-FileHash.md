# Get-FileHash

Get-FileHash computes the hash value for a file by using a specified hash algorithm. 
A hash value is a unique value that corresponds to the content of the file. 
Rather than identifying the contents of a file by its file name, extension, or other designation, a hash assigns a unique value to the contents of a file.

## Code

Get-FileHash computes the hash value for a file by using a specified hash algorithm. 
A hash value is a unique value that corresponds to the content of the file. 
Rather than identifying the contents of a file by its file name, extension, or other designation, a hash assigns a unique value to the contents of a file. 
File names and extensions can be changed without altering the content of the file, and without changing the hash value. 
Similarly, the file's content can be changed without changing the name or extension. 
However, changing even a single character in the contents of a file changes the hash value of the file.

The purpose of hash values is to provide a cryptographically-secure way to verify that the contents of a file have not been changed. 
While some hash algorithms, including MD5 and SHA1, are no longer considered secure against attack, the goal of a secure hash algorithm is to render it impossible to change the contents of a file-either by accident, or by malicious or unauthorized attempt-and maintain the same hash value. 
You can also use hash values to determine if two different files have exactly the same content. 
If the hash values of two files are identical, the contents of the files are also identical.

By default, the Get-FileHash cmdlet uses the SHA256 algorithm, although any hash algorithm that is supported by the target operating system can be used.

```Powershell
<#
MIT License
Copyright (c) 2016 powershell-kitchen.com
http://www.powershell-kitchen.com/page/mit-license
Published on: http://www.powershell-kitchen.com/post/Get-FileHash
#>

function Get-FileHash 
(
        [System.IO.FileInfo] $file = $(throw 'Usage: Get-FileHash [System.IO.FileInfo] [hashAlgorithm]'),
        [string] $algorithm = "sha256"
        )

  <#
  .SYNOPSIS
      Computes the hash value for a file by using a specified hash algorithm.
  .DESCRIPTION
      Get-FileHash computes the hash value for a file by using a specified hash algorithm. A hash value is a unique value that corresponds to the content of the file. Rather than identifying the contents of a file by its file name, extension, or other designation, a hash assigns a unique value to the contents of a file. File names and extensions can be changed without altering the content of the file, and without changing the hash value. Similarly, the file's content can be changed without changing the name or extension. However, changing even a single character in the contents of a file changes the hash value of the file.
      The purpose of hash values is to provide a cryptographically-secure way to verify that the contents of a file have not been changed. While some hash algorithms, including MD5 and SHA1, are no longer considered secure against attack, the goal of a secure hash algorithm is to render it impossible to change the contents of a file-either by accident, or by malicious or unauthorized attempt-and maintain the same hash value. You can also use hash values to determine if two different files have exactly the same content. If the hash values of two files are identical, the contents of the files are also identical.
      By default, the Get-FileHash cmdlet uses the SHA256 algorithm, although any hash algorithm that is supported by the target operating system can be used.
  .EXAMPLE
      Get-FileHash c:\temp\windows10.iso
  .EXAMPLE
      Get-FileHash c:\temp\windows10.iso "sha1"
  .PARAMETER file
      file to be used top generate the hash
  .PARAMETER algorithm
      Specifies the cryptographic hash function to use for computing the hash value of the contents of the specified file. A cryptographic hash function includes the property that it is not possible to find two distinct inputs that generate the same hash values. Hash functions are commonly used with digital signatures and for data integrity. Valid values for this parameter are SHA1, SHA256, SHA512, and MD5. If no value is specified, or if the parameter is omitted, the default value is SHA256.
      For security reasons, MD5 and SHA1, which are no longer considered secure, should only be used for simple change validation, and should not be used to generate hash values for files that require protection from attack or tampering.

  #>

{
    $hashAlgorithmProvider = $null;
    switch ($algorithm.ToLowerInvariant())
    {
      #md5 is not consider secure
      "md5" { $hashAlgorithmProvider = new-object "System.Security.Cryptography.MD5CryptoServiceProvider" }
      #sha1 is not consider secure
      "sha1" { $hashAlgorithmProvider = new-object "System.Security.Cryptography.SHA1Managed" }
      "sha256" { $hashAlgorithmProvider = new-object "System.Security.Cryptography.SHA256Managed" }
      "sha512" { $hashAlgorithmProvider = new-object "System.Security.Cryptography.SHA512Managed" }
      default { throw 'Unknown hash algorithm'}
    }
  $stream = $file.OpenRead();
  $hashByteArray = $hashAlgorithmProvider.ComputeHash($stream);
  $stream.Close();
  return [BitConverter]::ToString($hashByteArray).Replace("-", "");
}

```
