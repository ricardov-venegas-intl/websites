# Crypto-AES

This script shows how to encrypt and decrypt strings using AES.

Enjoy!

## Code
The Advanced Encryption Standard (AES), also known by its original name Rijndael is a specification for the encryption of electronic data established by the U.S. National Institute of Standards and Technology (NIST) in 2001.

---

```Powershell
<#
MIT License
Copyright (c) 2016 powershell-kitchen.com/ PS Cheff
http://www.powershell-kitchen.com/page/mit-license
#>

[System.Reflection.Assembly]::LoadWithPartialName("System.Security.Cryptography")
#[System.Security.Cryptography.RSACng]::Create()

function Get-AesFromKey([string] $key, $salt)
{

    $keyB = [Text.Encoding]::UTF8.GetBytes($key)
    $keyGenerator = New-Object "System.Security.Cryptography.Rfc2898DeriveBytes" ($keyB , $salt, 4095)
    $aes = New-Object System.Security.Cryptography.AesManaged
    $aes.KeySize =256
    $keyBytes = $keyGenerator.GetBytes($aes.KeySize/8)
    $ivBytes = $keyGenerator.GetBytes($aes.BlockSize/8)
    $aes.Key = $keyBytes
    $aes.IV =  $ivBytes 
    return $aes
}

  <#
  .SYNOPSIS
      Encrypts a string with AES 256. Returns an object with the encrypted message and other parameters

  .DESCRIPTION
      This function encrypts a string with an aes key. The key is generated from a password using Rfc2898DeriveBytes and a random salt

  .EXAMPLE
      Encrypt-String "Secret Password" "Secret message" | Export-Clixml "c:\MyFile.xml"

  .PARAMETER password
      password used to generate the aes key

  .PARAMETER message
      Message to encrypts
        
   .NOTES
        The returned object contains the following fields: Salt, Encrypted message, infor fields (KeyDerivation algorith, CryptoAlgorithm )     
    #>


function Encrypt-String([string] $password, [string] $message)
{
     $salt = new-object "byte[]" 32
     $rng = new-object Security.Cryptography.RNGCryptoServiceProvider
     $rng.GetBytes($salt)
     $aes = Get-AesFromKey $password $salt
     $encryptor = $aes.CreateEncryptor()
     $memoryStream = new-object "System.IO.MemoryStream"
     $cryptoStream = New-Object "System.Security.Cryptography.CryptoStream" ( $memoryStream,  $encryptor, [System.Security.Cryptography.CryptoStreamMode]::Write)
     $bytes = [System.Text.Encoding]::Unicode.GetBytes($message)
     $cryptoStream.Write($bytes, 0, $bytes.Length)
     $cryptoStream.FlushFinalBlock()
     $memoryStream.Flush()
     $result = new-Object PSObject
     $d = [ordered]@{Salt=$salt;EncryptedContent= [System.Convert]::ToBase64String($memoryStream.ToArray()); KeyDerivation="Rfc2898DeriveBytes:4095"; CryptoAlgorithm = "AES-256-CBC"}
     $result | Add-Member -NotePropertyMembers $d -TypeName "EncryptedMessage"
     $cryptoStream.Dispose();
     return $result;
}


  <#
  .SYNOPSIS
      Encrypts a string with AES. returns an object with the encrypted message and other parameters

  .DESCRIPTION
      This function encrypts a string with an aes key. The key is generated from a password using Rfc2898DeriveBytes


  .EXAMPLE
      $encryptedMessage =  Import-Clixml "c:\MyFile.xml"
      Decrypt-String "Secret Password" $encryptedMessage

  .PARAMETER password
      password used to generate the aes key

  .PARAMETER encryptedData
      Message to decrypt
        
    #>

function Decrypt-String([string] $key, $encryptedData)
{
     $bytes = [System.Convert]::FromBase64String($encryptedData.EncryptedContent)
     $buffer = new-object "byte[]" $bytes.Length
     $memoryStream = new-object "System.IO.MemoryStream" @( ,$bytes )

     $aes = Get-AesFromKey $key $encryptedData.Salt
     $decryptor = $aes.CreateDecryptor()
     $cryptoStream = New-Object "System.Security.Cryptography.CryptoStream" ( $memoryStream,  $decryptor, [System.Security.Cryptography.CryptoStreamMode]::Read)
     $len = $cryptoStream.Read($buffer, 0, $buffer.Length)
     return [System.Text.Encoding]::Unicode.GetString($buffer, 0, $len)
}

```

