//  *******************************************************************************
//  *  Licensed under the Apache License, Version 2.0 (the "License");
//  *  you may not use this file except in compliance with the License.
//  *  You may obtain a copy of the License at
//  *
//  *  http://www.apache.org/licenses/LICENSE-2.0
//  *
//  *   Unless required by applicable law or agreed to in writing, software
//  *   distributed under the License is distributed on an "AS IS" BASIS,
//  *   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//  *   See the License for the specific language governing permissions and
//  *   limitations under the License.
//  ******************************************************************************

using System.Collections.Generic;

namespace SharpCommunication.Codec
{
    internal static class TalkerHelper
    {
        internal static Talker GetTalker(string messageType)
        {
            if (messageType[0] == 'P')
                return Talker.ProprietaryCode;
            return _talkerLookupTable.ContainsKey(messageType.Substring(0, 2)) ? _talkerLookupTable[messageType.Substring(0, 2)] : Talker.Unknown;
        }

        private static readonly Dictionary<string, Talker> _talkerLookupTable = new Dictionary<string, Talker>()
        {
            {"AB", Talker.IndependentAisBaseStation                 },
            {"AD", Talker.DependentAisBaseStation                   },
            {"AG", Talker.HeadingTrackControllerGeneral             },
            {"AP", Talker.HeadingTrackControllerMagnetic            },
            {"AI", Talker.MobileClassAorBaisStation                 },
            {"AN", Talker.AisAidstoNavigationStation                },
            {"AR", Talker.AisReceivingStation                       },
            {"AS", Talker.AisStation                                },
            {"AT", Talker.AisTransmittingStation                    },
            {"AX", Talker.AisSimplexRepeaterStation                 },
            {"BI", Talker.BilgeSystems                              },
            {"CD", Talker.DigitalSelectiveCalling                   },
            {"CR", Talker.DataReceiver                              },
            {"CS", Talker.Satellite                                 },
            {"CT", Talker.RadioTelephoneMfhf                        },
            {"CV", Talker.RadioTelephoneVhf                         },
            {"CX", Talker.ScanningReceiver                          },
            {"DE", Talker.DeccaNavigator                            },
            {"DF", Talker.DirectionFinder                           },
            {"DU", Talker.DuplexRepeaterStation                     },
            {"EC", Talker.ElectronicChartSystem                     },
            {"EI", Talker.ElectronicChartDisplayInformationSystem   },
            {"EP", Talker.EmergencyPositionIndicatingBeacon         },
            {"ER", Talker.EngineRoomMonitoringSystems               },
            {"FD", Talker.FireDoorControllerMonitoringPoint         },
            {"FE", Talker.FireExtinguisherSystem                    },
            {"FR", Talker.FireDetectionPoint                        },
            {"FS", Talker.FireSprinklerSystem                       },
            {"GA", Talker.GalileoPositioningSystem                  },
            {"GL", Talker.GlonassReceiver                           },
            {"GN", Talker.GlobalNavigationSatelliteSystem           },
            {"GP", Talker.GlobalPositioningSystem                   },
            {"HC", Talker.CompassMagnetic                           },
            {"HE", Talker.GyroNorthSeeking                          },
            {"HF", Talker.Fluxgate                                  },
            {"HN", Talker.GyroNonNorthSeeking                       },
            {"HD", Talker.HullDoorControllerMonitoringPanel         },
            {"HS", Talker.HullStressMonitoring                      },
            {"II", Talker.IntegratedInstrumentation                 },
            {"IN", Talker.IntegratedNavigation                      },
            {"LC", Talker.LoranC                                    },
            {"P ", Talker.ProprietaryCode                           },
            {"RA", Talker.RadarAndOrRadarPlotting                   },
            {"RC", Talker.PropulsionMachineryIncludingRemoteControl },
            {"SA", Talker.PhysicalShoreAisStation                   },
            {"SD", Talker.SounderDepth                              },
            {"SG", Talker.SteeringGearSteeringEngine                },
            {"SN", Talker.ElectronicPositioningSystem               },
            {"SS", Talker.SounderScanning                           },
            {"TI", Talker.TurnRateIndicator                         },
            {"UP", Talker.MicroprocessorController                  },
            {"U0", Talker.UserId0                                   },
            {"U1", Talker.UserId1                                   },
            {"U2", Talker.UserId2                                   },
            {"U3", Talker.UserId3                                   },
            {"U4", Talker.UserId4                                   },
            {"U5", Talker.UserId5                                   },
            {"U6", Talker.UserId6                                   },
            {"U7", Talker.UserId7                                   },
            {"U8", Talker.UserId8                                   },
            {"U9", Talker.UserId9                                   },
            {"VD", Talker.Doppler                                   },
            {"VM", Talker.SpeedLogWaterMagnetic                     },
            {"VW", Talker.SpeedLogWaterMechanical                   },
            {"VR", Talker.VoyageDataRecorder                        },
            {"WD", Talker.WatertightDoorControllerMonitoringPanel   },
            {"WI", Talker.WeatherInstruments                        },
            {"WL", Talker.WaterLevelDetectionSystems                },
            {"YX", Talker.Transducer                                },
            {"ZA", Talker.AtomicsClock                              },
            {"ZC", Talker.Chronometer                               },
            {"ZQ", Talker.Quartz                                    },
            {"ZV", Talker.RadioUpdate                               },

        };

    }

    /// <summary>
    /// Talker Identifier
    /// </summary>
    public enum Talker
    {
        /// <summary>
        /// Unrecognized Talker ID
        /// </summary>
        Unknown,
        /// <summary>Independent AIS Base Station</summary>
        IndependentAisBaseStation, // = AB
        /// <summary>Dependent AIS Base Station</summary>
        DependentAisBaseStation, // = AD 
        /// <summary>Heading Track Controller (Autopilot) - General</summary>
        HeadingTrackControllerGeneral, // = AG
        /// <summary>Heading Track Controller (Autopilot) - Magnetic</summary>
        HeadingTrackControllerMagnetic, // = AP
        /// <summary>Mobile Class A or B AIS Station</summary>
        MobileClassAorBaisStation, // = AI
        /// <summary>AIS Aids to Navigation Station </summary>
        AisAidstoNavigationStation, // = AN
        /// <summary>AIS Receiving Station</summary>
        AisReceivingStation, // = AR
        /// <summary>AIS Station (ITU_R M1371,  (“Limited Base Station’)</summary>
        AisStation, // = AS
        /// <summary>AIS Transmitting Station</summary>
        AisTransmittingStation, // = AT
        /// <summary>AIS Simplex Repeater Station</summary>
        AisSimplexRepeaterStation, // = AX
        /// <summary>Bilge Systems</summary>
        BilgeSystems, // = BI
        /// <summary></summary>
        DigitalSelectiveCalling, // = CD
        /// <summary></summary>
        DataReceiver, // = CR
        /// <summary></summary>
        Satellite, // = CS
        /// <summary></summary>
        RadioTelephoneMfhf, // = CT
        /// <summary></summary>
        RadioTelephoneVhf, // = CV
        /// <summary></summary>
        ScanningReceiver, // = CX
        /// <summary></summary>
        DeccaNavigator, // = DE
        /// <summary></summary>
        DirectionFinder, // = DF
        /// <summary></summary>
        DuplexRepeaterStation, // = DU
        /// <summary></summary>
        ElectronicChartSystem, // = EC
        /// <summary></summary>
        ElectronicChartDisplayInformationSystem, // = EI
        /// <summary></summary>
        EmergencyPositionIndicatingBeacon, // = EP
        /// <summary></summary>
        EngineRoomMonitoringSystems, // = ER
        /// <summary></summary>
        FireDoorControllerMonitoringPoint, // = FD
        /// <summary></summary>
        FireExtinguisherSystem, // = FE
        /// <summary></summary>
        FireDetectionPoint, // = FR
        /// <summary></summary>
        FireSprinklerSystem, // = FS
        /// <summary>Galileo Positioning System</summary>
        GalileoPositioningSystem, // = GA
        /// <summary>GLONASS Receiver</summary>
        GlonassReceiver, // = GL
        /// <summary>Global Navigation Satellite System (GNSS</summary>
        GlobalNavigationSatelliteSystem, // = GN
        /// <summary>Global Positioning System (GPS)</summary>
        GlobalPositioningSystem, // = GPS
        /// <summary>Heading Sensor - Compass, Magnetic</summary>
        CompassMagnetic, // = HC
        /// <summary>Heading Sensor - Gyro, North Seeking</summary>
        GyroNorthSeeking, // = HE
        /// <summary>Heading Sensor - Fluxgate</summary>
        Fluxgate, // = HF
        /// <summary>Heading Sensor - Gyro, Non-North Seeking</summary>
        GyroNonNorthSeeking, // = HN
        /// <summary>Hull Door Controller/Monitoring Panel</summary>
        HullDoorControllerMonitoringPanel, // = HD
        /// <summary>Hull Stress Monitoring</summary>
        HullStressMonitoring, // = HS
        /// <summary>Integrated Instrumentation</summary>
        IntegratedInstrumentation, // = II
        /// <summary>Integrated Navigation</summary>
        IntegratedNavigation, // = IN
        /// <summary>Loran C</summary>
        LoranC, // = LC
        /// <summary></summary>
        ProprietaryCode, // = P
        /// <summary></summary>
        RadarAndOrRadarPlotting, // = RA
        /// <summary></summary>
        PropulsionMachineryIncludingRemoteControl, // = RC
        /// <summary></summary>
        PhysicalShoreAisStation, // = SA
        /// <summary></summary>
        SounderDepth, // = SD
        /// <summary></summary>
        SteeringGearSteeringEngine, // = SG
        /// <summary></summary>
        ElectronicPositioningSystem, // = SN
        /// <summary></summary>
        SounderScanning, // = SS
        /// <summary></summary>
        TurnRateIndicator, // = TI
        /// <summary></summary>
        MicroprocessorController, // = UP
        /// <summary>User configured talker identifier</summary>
        UserId0, // = U0
        /// <summary>User configured talker identifier</summary>
        UserId1, // = U1
        /// <summary>User configured talker identifier</summary>
        UserId2, // = U2
        /// <summary>User configured talker identifier</summary>
        UserId3, // = U3
        /// <summary>User configured talker identifier</summary>
        UserId4, // = U4
        /// <summary>User configured talker identifier</summary>
        UserId5, // = U5
        /// <summary>User configured talker identifier</summary>
        UserId6, // = U6
        /// <summary>User configured talker identifier</summary>
        UserId7, // = U7
        /// <summary>User configured talker identifier</summary>
        UserId8, // = U8
        /// <summary>User configured talker identifier</summary>
        UserId9, // = U9
        /// <summary>Velocity sensor - Doppler</summary>
        Doppler, // = VD
        /// <summary>Velocity sensor - Speed Log, Water, Magnetic</summary>
        SpeedLogWaterMagnetic, // = VM
        /// <summary>Velocity sensor - Speed Log, Water Mechanical</summary>
        SpeedLogWaterMechanical, // = VW
        /// <summary>Voyage Data Recorder</summary>
        VoyageDataRecorder, // = VR
        /// <summary>Watertight Door Controller/Monitoring Panel</summary>
        WatertightDoorControllerMonitoringPanel, // = WD
        /// <summary>Weather Instruments</summary>
        WeatherInstruments, // = WI
        /// <summary>Water Level Detection Systems </summary>
        WaterLevelDetectionSystems, // = WL
        /// <summary>Transducer</summary>
        Transducer, // = YX
        /// <summary>Time keeper - Atomics Clock</summary>
        AtomicsClock, // = ZA
        /// <summary>Time keeper - Chronometer</summary>
        Chronometer, // = ZC
        /// <summary>Time keeper - Quartz</summary>
        Quartz, // = ZQ
        /// <summary>Time keeper - Radio Update</summary>
        RadioUpdate, // = ZV

    }
}
