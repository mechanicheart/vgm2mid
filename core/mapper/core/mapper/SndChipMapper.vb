Namespace Core
    Namespace MMap
        Public MustInherit Class SndChipMapper : Inherits MemoryMapper
            Public Enum PSGMapperID
                FINEA
                COARSEA
                FINEB
                COARSEB
                FINEC
                COARSEC
                NOISE
                MIXTONE
                MIXNOISE
                MIXIO
                ENVA
                ENVAE
                ENVB
                ENVBE
                ENVC
                ENVCE
                FINEENV
                COARSEENV
                ENVSHAPE
                IOA
                IOB
                Length
            End Enum

            Public Enum FMMapperID
                TEST
                TIMERA
                TIMERB
                LOAD
                ENABLE
                RESET
                MODE
                CHAN
                SLOT
                FB
                CON
                ' LFO
                AME
                AMS
                PMS
                ' Operator
                AR
                D1R
                D2R
                SR
                RR
                D1L
                D2L
                TL
                SL
                KS
                MUL
                DT1
                DT2
                SSGEG
                ' Channel
                FNUM
                BLOCK
                CH3FNUM
                CH3BLOCK
                Length
            End Enum

        End Class

        Namespace PSG
            Public Class PSGMapper : Inherits SndChipMapper
                Private PSGMMapperUnit() As MapperUnit
                Public Sub New()
                    ReDim Mapper(PSGMapperID.Length)
                    Mapper = {
                        New MapperUnit(True, &H0, 0, 0, 8, False, 0, 0, 0, PSGMapperID.FINEA),
                        New MapperUnit(True, &H1, 0, 0, 4, False, 0, 0, 0, PSGMapperID.COARSEA),
                        New MapperUnit(True, &H2, 0, 0, 8, False, 0, 0, 0, PSGMapperID.FINEB),
                        New MapperUnit(True, &H3, 0, 0, 4, False, 0, 0, 0, PSGMapperID.COARSEB),
                        New MapperUnit(True, &H4, 0, 0, 8, False, 0, 0, 0, PSGMapperID.FINEC),
                        New MapperUnit(True, &H5, 0, 0, 4, False, 0, 0, 0, PSGMapperID.COARSEC),
                        New MapperUnit(True, &H6, 0, 0, 5, False, 0, 0, 0, PSGMapperID.NOISE),
                        New MapperUnit(True, &H7, 0, 0, 3, False, 0, 0, 0, PSGMapperID.MIXTONE),
                        New MapperUnit(True, &H7, 0, 3, 3, False, 0, 0, 0, PSGMapperID.MIXNOISE),
                        New MapperUnit(True, &H7, 0, 6, 2, False, 0, 0, 0, PSGMapperID.MIXIO),
                        New MapperUnit(True, &H8, 0, 0, 4, False, 0, 0, 0, PSGMapperID.ENVA)
                    }
                End Sub

            End Class
        End Namespace

        Namespace OPM
            Public Class OPMMapper : Inherits SndChipMapper
                Public Sub New()

                End Sub
            End Class
        End Namespace

        Namespace OPN
            Public Class OPNMapper : Inherits SndChipMapper

                Public Sub New()
                    ReDim Mapper(FMMapperID.Length)
                    Mapper = {
                        New MapperUnit(True, &H21, 0, 0, 8, False, 0, 0, 0, FMMapperID.TEST), ' Test
                        New MapperUnit(True, &H24, 0, 0, 8, True, &H25, 0, 2, FMMapperID.TIMERA), ' Timer A
                        New MapperUnit(True, &H26, 0, 0, 8, False, 0, 0, 0, FMMapperID.TIMERB), ' Timer B
                        New MapperUnit(True, &H27, 0, 0, 2, False, 0, 0, 0, FMMapperID.LOAD), ' Load
                        New MapperUnit(True, &H27, 0, 2, 2, False, 0, 0, 0, FMMapperID.ENABLE), ' Enable
                        New MapperUnit(True, &H27, 0, 4, 2, False, 0, 0, 0, FMMapperID.RESET), ' Reset
                        New MapperUnit(True, &H27, 0, 6, 2, False, 0, 0, 0, FMMapperID.MODE), ' Mode
                        New MapperUnit(True, &H28, 0, 0, 2, False, 0, 0, 0, FMMapperID.CHAN), ' Chan
                        New MapperUnit(True, &H28, 0, 4, 4, False, 0, 0, 0, FMMapperID.SLOT), ' Slot
                        New MapperUnit(True, &H30, 4, 0, 4, False, 0, 0, 0, FMMapperID.MUL), ' Multiple
                        New MapperUnit(True, &H30, 4, 4, 3, False, 0, 0, 0, FMMapperID.DT1), ' Detune
                        New MapperUnit(True, &H40, 4, 0, 7, False, 0, 0, 0, FMMapperID.TL), ' Total Level
                        New MapperUnit(True, &H50, 4, 0, 5, False, 0, 0, 0, FMMapperID.AR), ' Attack Rate
                        New MapperUnit(True, &H50, 4, 6, 2, False, 0, 0, 0, FMMapperID.KS), ' Key Scale
                        New MapperUnit(True, &H60, 4, 0, 5, False, 0, 0, 0, FMMapperID.D1R), ' Decay Rate
                        New MapperUnit(True, &H70, 4, 0, 5, False, 0, 0, 0, FMMapperID.SR), ' Sustain Rate
                        New MapperUnit(True, &H80, 4, 0, 4, False, 0, 0, 0, FMMapperID.RR), ' Release Rate
                        New MapperUnit(True, &H80, 4, 4, 4, False, 0, 0, 0, FMMapperID.SL), ' Sustain Level
                        New MapperUnit(True, &H90, 4, 4, 4, False, 0, 0, 0, FMMapperID.SSGEG), ' SSG Envelope
                        New MapperUnit(True, &HA0, 0, 0, 8, True, &HA4, 0, 3, FMMapperID.FNUM), ' FNumber
                        New MapperUnit(True, &HA4, 0, 3, 3, False, 0, 0, 0, FMMapperID.BLOCK), ' Octave Block
                        New MapperUnit(True, &HA8, 0, 0, 8, True, &HAC, 0, 3, FMMapperID.CH3FNUM), ' 3CH FNumber
                        New MapperUnit(True, &HAC, 0, 3, 3, False, 0, 0, 0, FMMapperID.CH3BLOCK), ' 3CH Octave Block
                        New MapperUnit(True, &HB0, 0, 0, 3, False, 0, 0, 0, FMMapperID.CON), 'Connect
                        New MapperUnit(True, &HB0, 0, 3, 3, False, 0, 0, 0, FMMapperID.FB) 'FeedBack
                    }
                End Sub


            End Class
            Public Class OPNAMapper : Inherits SndChipMapper

            End Class

            Public Class OPNBMapper : Inherits SndChipMapper

            End Class

            Public Class OPN2Mapper : Inherits SndChipMapper

            End Class

            Public Class OPNCMapper : Inherits SndChipMapper

            End Class
        End Namespace
    End Namespace
End Namespace
