#import <Foundation/Foundation.h>
#import <AuthenticationServices/ASWebAuthenticationSession.h>
#import <UnityFramework/UnityFramework-Swift.h>

extern "C" {
    void launch_inapp_browser(const char *url) {
        [InAppBrowser launch:[NSString stringWithUTF8String:url]];
    }

    void close_inapp_browser() {
        [InAppBrowser close];
    }
}
