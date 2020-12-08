Namespace Instrument
    Public MustInherit Class Instrument
        Private szInstrName As String
        Public Property Name() As String
            Get
                Return szInstrName
            End Get
            Set(val As String)
                szInstrName = val
            End Set
        End Property

        Public MustOverride Function CompareTo(instr As Instrument) As Boolean
    End Class

    Public Class FMInstrument : Inherits Instrument
        Public Structure stFMInstrOp
            Dim iAttrAR As Integer
            Dim iAttrD1R As Integer
            Dim iAttrD2R As Integer
            Dim iAttrSR As Integer
            Dim iAttrRR As Integer

            Dim iAttrD1L As Integer
            Dim iAttrD2L As Integer
            Dim iAttrTL As Integer
            Dim iAttrSL As Integer

            Dim iAttrKS As Integer
            Dim iAttrMUL As Integer
            Dim iAttrD1T As Integer
            Dim iAttrD2T As Integer

            Dim iAttrSSGEG As Integer
            Dim iAttrAME As Integer
        End Structure

        Public FMInstrOp() As stFMInstrOp
        Private iFMInstrOpCount As Integer

        Private iAttrSlot As Integer
        Private iAttrFB As Integer
        Private iAttrCON As Integer


        Public Property AR(id As Integer) As Integer
            Get
                Return FMInstrOp(id).iAttrAR
            End Get
            Set(val As Integer)
                FMInstrOp(id).iAttrAR = val
            End Set
        End Property

        Public Property D1R(id As Integer) As Integer
            Get
                Return FMInstrOp(id).iAttrD1R
            End Get
            Set(val As Integer)
                FMInstrOp(id).iAttrD1R = val
            End Set
        End Property

        Public Property D2R(id As Integer) As Integer
            Get
                Return FMInstrOp(id).iAttrD2R
            End Get
            Set(val As Integer)
                FMInstrOp(id).iAttrD2R = val
            End Set
        End Property

        Public Property SR(id As Integer) As Integer
            Get
                Return FMInstrOp(id).iAttrSR
            End Get
            Set(val As Integer)
                FMInstrOp(id).iAttrSR = val
            End Set
        End Property

        Public Property RR(id As Integer) As Integer
            Get
                Return FMInstrOp(id).iAttrRR
            End Get
            Set(val As Integer)
                FMInstrOp(id).iAttrRR = val
            End Set
        End Property

        Public Property D1L(id As Integer) As Integer
            Get
                Return FMInstrOp(id).iAttrD1L
            End Get
            Set(val As Integer)
                FMInstrOp(id).iAttrD1L = val
            End Set
        End Property

        Public Property D2L(id As Integer) As Integer
            Get
                Return FMInstrOp(id).iAttrD2L
            End Get
            Set(val As Integer)
                FMInstrOp(id).iAttrD2L = val
            End Set
        End Property

        Public Property TL(id As Integer) As Integer
            Get
                Return FMInstrOp(id).iAttrTL
            End Get
            Set(val As Integer)
                FMInstrOp(id).iAttrTL = val
            End Set
        End Property

        Public Property SL(id As Integer) As Integer
            Get
                Return FMInstrOp(id).iAttrSL
            End Get
            Set(val As Integer)
                FMInstrOp(id).iAttrSL = val
            End Set
        End Property

        Public Property KS(id As Integer) As Integer
            Get
                Return FMInstrOp(id).iAttrKS
            End Get
            Set(val As Integer)
                FMInstrOp(id).iAttrKS = val
            End Set
        End Property

        Public Property MUL(id As Integer) As Integer
            Get
                Return FMInstrOp(id).iAttrMUL
            End Get
            Set(val As Integer)
                FMInstrOp(id).iAttrMUL = val
            End Set
        End Property

        Public Property D1T(id As Integer) As Integer
            Get
                Return FMInstrOp(id).iAttrD1T
            End Get
            Set(val As Integer)
                FMInstrOp(id).iAttrD1T = val
            End Set
        End Property

        Public Property D2T(id As Integer) As Integer
            Get
                Return FMInstrOp(id).iAttrD2T
            End Get
            Set(val As Integer)
                FMInstrOp(id).iAttrD2T = val
            End Set
        End Property

        Public Property SSGEG(id As Integer) As Integer
            Get
                Return FMInstrOp(id).iAttrSSGEG
            End Get
            Set(val As Integer)
                FMInstrOp(id).iAttrSSGEG = val
            End Set
        End Property

        Public Property AME(id As Integer) As Integer
            Get
                Return FMInstrOp(id).iAttrAME
            End Get
            Set(val As Integer)
                FMInstrOp(id).iAttrAME = val
            End Set
        End Property

        Public Overrides Function CompareTo(instr As Instrument) As Boolean
            Return True
        End Function

        ' Compatible With Previous Work
        Public Function ToVoiceStruct() As Form1.Voice_Struct
            Dim vs As Form1.Voice_Struct = New Form1.Voice_Struct

            For i As Integer = 0 To iFMInstrOpCount
                vs.Op(i).AR = AR(i)
                vs.Op(i).D1R = D1R(i)
                vs.Op(i).D2R = D2R(i)
                vs.Op(i).SR = SR(i)
                vs.Op(i).RR = RR(i)
                vs.Op(i).D1L = D1L(i)
                vs.Op(i).TL = TL(i)
                vs.Op(i).KS = KS(i)
                vs.Op(i).MUL = MUL(i)
                vs.Op(i).DT1 = D1T(i)
                vs.Op(i).DT2 = D2T(i)
                vs.Op(i).SSGEG = SSGEG(i)
                vs.Op(i).AME = AME(i)
            Next

            Return vs
        End Function

    End Class

    Public Class PSGInstrument : Inherits Instrument
        Public Overrides Function CompareTo(instr As Instrument) As Boolean
            Return True
        End Function
    End Class

End Namespace
