using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IridiUpload.Logic.Iridi.Cloud
{
    class Interface
    {
        public class TObjectFolder
        {
            public string name { get; set; }
            public string id { get; set; }
            public int position { get; set; }
            public TObject[] objects { get; set; }
        }

        public class TProject
        {
            public string name { get; set; }
            public string id { get; set; }
            public int position { get; set; }
        }

        public class TObject
        {
            public string name { get; set; }
            public string id { get; set; }
            public int position { get; set; }
            public TProject[] projects { get; set; }
        }

        public class TVOTES
        {

        }
        public class TVOTE
        {
            public string[] VOTES { get; set; }
        }
        public class TPOLICY
        {
            public string SESSION_TIMEOUT { get; set; }
            public string SESSION_IP_MASK { get; set; }
            public string MAX_STORE_NUM { get; set; }
            public string STORE_IP_MASK { get; set; }
            public string STORE_TIMEOUT { get; set; }
            public string CHECKWORD_TIMEOUT { get; set; }
            public string PASSWORD_LENGTH { get; set; }
            public string PASSWORD_UPPERCASE { get; set; }
            public string PASSWORD_LOWERCASE { get; set; }
            public string PASSWORD_DIGITS { get; set; }
            public string PASSWORD_PUNCTUATION { get; set; }
            public string LOGIN_ATTEMPTS { get; set; }
            public string BLOCK_LOGIN_ATTEMPTS { get; set; }
            public string BLOCK_TIME { get; set; }
            public string PASSWORD_REQUIREMENTS { get; set; }
        }
        public class TSESS_AUTH
        {
            public TPOLICY POLICY { get; set; }
            public string AUTHORIZED { get; set; }
            public string USER_ID { get; set; }
            public string LOGIN { get; set; }
            public string LOGIN_COOKIES { get; set; }
            public string EMAIL { get; set; }
            public string PASSWORD_HASH { get; set; }
            public string TITLE { get; set; }
            public string NAME { get; set; }
            public string FIRST_NAME { get; set; }
            public string SECOND_NAME { get; set; }
            public string LAST_NAME { get; set; }
            public string PERSONAL_PHOTO { get; set; }
            public string PERSONAL_GENDER { get; set; }
            public string PERSONAL_WWW { get; set; }
            public string EXTERNAL_AUTH_ID { get; set; }
            public string XML_ID { get; set; }
            public string ADMIN { get; set; }
            public string AUTO_TIME_ZONE { get; set; }
            public string TIME_ZONE { get; set; }
            public string TIME_ZONE_OFFSET { get; set; }
            public string APPLICATION_ID { get; set; }
            public string BX_USER_ID { get; set; }
            public string[] GROUPS { get; set; }
            public string CONTROLLER_ADMIN { get; set; }
            public string SESSION_HASH { get; set; }
            public string STORED_AUTH_ID { get; set; }
        }
        public class Tsess
        {
            public TSESS_AUTH SESS_AUTH { get; set; }
            public string SESS_IP { get; set; }
            public string SESS_TIME { get; set; }
            public string BX_SESSION_SIGN { get; set; }
            public string SESS_ID_TIME { get; set; }
            public string BX_LOGIN_NEED_CAPTCHA { get; set; }
            public TVOTE VOTE { get; set; }
            public string[] AUTH_ACTIONS_PERFORMED { get; set; }
            public string[] SESS_OPERATIONS { get; set; }
            public string fixed_session_id { get; set; }
            public string referer1 { get; set; }
            public string referer2 { get; set; }
            public string referer3 { get; set; }
            public string SESS_SEARCHER_CHECK_ACTIVITY { get; set; }
            public string[] SESS_GRABBER_DEFENCE_STACK { get; set; }
            public string SESS_SEARCHER_ID { get; set; }
            public string SESS_COUNTRY_ID { get; set; }
            public string SESS_CITY_ID { get; set; }
            public string SESS_ADV_ID { get; set; }
            public string SESS_LAST_ADV_ID { get; set; }
            public string SESS_GUEST_NEW { get; set; }
            public string SESS_GUEST_ID { get; set; }
            public string SESS_GUEST_FAVORITES { get; set; }
            public string SESS_LAST_USER_ID { get; set; }
            public string SESS_SESSION_ID { get; set; }
            public string SESS_HTTP_REFERER { get; set; }
            public string SESS_LAST_PROTOCOL { get; set; }
            public string SESS_LAST_PORT { get; set; }
            public string SESS_LAST_HOST { get; set; }
            public string SESS_LAST_URI { get; set; }
            public string SESS_LAST_PAGE { get; set; }
            public string SESS_LAST_DIR { get; set; }
            public string BX_SESSION_COUNTER { get; set; }
            public string BX_SESSION_TERMINATE_TIME { get; set; }
            public string IR_BACKURL { get; set; }
        }
        public class TIridiAuth
        {
            public int status { get; set; }
            public string backurl { get; set; }
            public Tsess sess { get; set; }
            public int uid { get; set; }
        }

    }
}
