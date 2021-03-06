﻿using System;

namespace Bandwidth.Net.Data
{
    public class Gather
    {
        public string Id { get; set; }
        public string State { get; set; }
        public string Reason { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? CompletedTime { get; set; }
        public Uri Call { get; set; }
        public string Digits { get; set; }
    }

    public class CreateGather
    {
        public int? MaxDigits { get; set; }
        public double? InterDigitTimeout { get; set; }
        public string TerminatingDigits { get; set; }
        public bool? SuppressDtmf { get; set; }
        public CreateGatherPromt Promt { get; set; }
    }

    public class CreateGatherPromt
    {
        public string Sentence { get; set; }
        public Gender? Gender { get; set; }
        public string Locale { get; set; }
        public Uri FileUrl { get; set; }
        public bool? LoopEnabled { get; set; }
        public bool? Bargeable { get; set; }
    }
}