﻿namespace Oje.Sanab.Models.View.Api
{
    public class SwitchTokenSuccessResult: SanabFirstError
    {
        public string access_token { get; set; }
        public string scope { get; set; }
        public string token_type { get; set; }
        public string expires_in { get; set; }
    }
}
