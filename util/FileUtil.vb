Imports System.IO
Imports System.IO.Compression

Namespace Util
    Public Class FileUtil
        ' GetFileAttributes Declaration
        ' Private Declare Function GetFileAttributes Lib "kernel32" Alias "GetFileAttributesA" (ByVal lpFileName As String) As Long
        ' Private Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (Destination As Long, Source As Byte(), ByVal Length As Long)
        ' Private Declare Function InitDecompression Lib "gzip.dll" () As Long
        ' Private Declare Function CreateDecompression Lib "gzip.dll" (ByRef context As Long, ByVal Flags As Long) As Long
        ' Declare Function DestroyDecompression Lib "gzip.dll" (ByRef context As Long) As Long
        ' Private Declare Function Decompress Lib "gzip.dll" (ByVal context As Long, inBytes As Byte, ByVal input_size As Long, outBytes As Byte, ByVal output_size As Long, ByRef input_used As Long, ByRef output_used As Long) As Long

        Private Const FILEATTR_DIRECTORY = &H10
        Private Const FILEATTR_INVALID = &H1

        Shared Function IsFileExists(file As String) As Boolean
            ' Dim attr As Long = GetFileAttributes(file)
            ' Return (attr <> FILEATTR_INVALID) And (Not (attr And FILEATTR_DIRECTORY))
            Return IO.File.Exists(file)
        End Function

        Shared Function GZRead(file As String, ByRef szOutStream As Byte()) As Boolean
            Dim hFile As FileStream = New FileStream(file, FileMode.Open)
            Dim hGZFile As GZipStream = New GZipStream(hFile, CompressionMode.Decompress)
            Dim hMemBuffer As MemoryStream = New MemoryStream()

            hGZFile.CopyTo(hMemBuffer)
            szOutStream = hMemBuffer.ToArray()

            hFile.Close() : hMemBuffer.Close() : hGZFile.Close()

            Return True
        End Function

        Shared Function GZRead(file As String) As GZipStream
            Dim hFile As FileStream = New FileStream(file, FileMode.Open)
            Dim hGZFile As GZipStream = New GZipStream(hFile, CompressionMode.Decompress)
            Return hGZFile
        End Function

        Shared Function LoadFileMemory(file As String) As MemoryStream
            If (IsFileExists(file) <> True) Then Return Nothing

            Dim hFile, hTmp As FileStream
            Dim hGZFile As GZipStream
            Dim hMem As MemoryStream = New MemoryStream()

            Dim szBuffer(2) As Byte

            hTmp = New FileStream(file, FileMode.Open)
            hTmp.Read(szBuffer, 0, 3)
            hTmp.Close()
            If ((szBuffer(0) = &H1F) And (szBuffer(1) = &H8B)) Then
                hGZFile = GZRead(file)
                hGZFile.CopyTo(hMem)
                hGZFile.Close()
            Else
                hFile = New FileStream(file, FileMode.Open)
                hFile.CopyTo(hMem)
                hFile.Close()
            End If

            Return hMem
        End Function
    End Class

End Namespace