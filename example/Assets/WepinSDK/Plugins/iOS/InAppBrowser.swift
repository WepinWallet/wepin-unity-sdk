import AuthenticationServices
import SafariServices
import UIKit
import WebKit

@objc public class InAppBrowser: UIViewController {

    public static let instance = InAppBrowser();
    @objc public func call(_ url: String) {
        if #available(iOS 9.0, *) {
            let safariVC = SFSafariViewController(url: URL(string: url)!)
            //safariVC.delegate = self

            if let rootViewController = UIApplication.shared.keyWindow?.rootViewController {
                rootViewController.present(safariVC, animated: true, completion: nil)
            }
        }
    }
    
    @objc public static func launch(_ url: String) {
        instance.call(url);
    }

    @objc public static func close() {
        if let rootViewController = UIApplication.shared.keyWindow?.rootViewController {
            rootViewController.presentedViewController?.dismiss(animated: true, completion: nil)
        }
    }
}

@available(iOS 12.0, *)
extension InAppBrowser: ASWebAuthenticationPresentationContextProviding {
    public func presentationAnchor(for session: ASWebAuthenticationSession) -> ASPresentationAnchor {
        return UnityFramework.getInstance().appController().window;
    }
}
