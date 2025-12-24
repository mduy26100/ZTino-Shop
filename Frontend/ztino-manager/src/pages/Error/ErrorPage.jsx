import { Button, Result } from "antd";
import { useRouteError, useNavigate, isRouteErrorResponse } from "react-router-dom";

const ErrorPage = () => {
  const error = useRouteError();
  const navigate = useNavigate();

  const getErrorContent = () => {
    let status = "500";
    let title = "Something went wrong";
    let subTitle = "Sorry, an unexpected error has occurred.";

    if (isRouteErrorResponse(error)) {
      switch (error.status) {
        case 404:
          status = "404";
          title = "404";
          subTitle = "Sorry, the page you visited does not exist.";
          break;
        case 403:
          status = "403";
          title = "403";
          subTitle = "Sorry, you are not authorized to access this page.";
          break;
        case 401:
          status = "403";
          title = "401";
          subTitle = "Sorry, you need to login to access this page.";
          break;
        case 500:
          status = "500";
          title = "500";
          subTitle = "Sorry, something went wrong on our server.";
          break;
        default:
          status = "error";
          title = error.status.toString();
          subTitle = error.statusText || "Unknown Error";
      }
    } else if (error instanceof Error) {
      status = "error";
      title = "Application Error";
      subTitle = error.message;
    }

    return { status, title, subTitle };
  };

  const { status, title, subTitle } = getErrorContent();

  return (
    <div className="flex items-center justify-center min-h-screen bg-gray-50 p-4">
      <Result
        status={status}
        title={title}
        subTitle={subTitle}
        extra={[
          <Button key="back" onClick={() => navigate("/dashboard")}>
            Back to Dashboard
          </Button>
        ]}
      >
      </Result>
    </div>
  );
};

export default ErrorPage;