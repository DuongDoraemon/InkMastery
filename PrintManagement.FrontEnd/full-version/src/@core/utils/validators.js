import { isEmpty, isEmptyArray, isNullOrUndefined } from "./index";

// 👉 Required Validator
export const requiredValidator = (value) => {
  if (isNullOrUndefined(value) || isEmptyArray(value) || value === false)
    return "This field is required";

  return !!String(value).trim().length || "This field is required";
};

// 👉 Email Validator
export const emailValidator = (value) => {
  if (isEmpty(value)) return true;

  const re = /^[^\W_][\w\.-]*[^\W_]@[^\W_][\w\.-]*[^\W_]\.[a-zA-Z]{2,}$/; // Kiểm tra không chứa dấu câu trước @

  if (Array.isArray(value))
    return (
      value.every((val) => re.test(String(val))) ||
      "The Email field must be a valid email"
    );

  return re.test(String(value)) || "The Email field must be a valid email";
};

// 👉 Password Validator
export const passwordValidator = (password) => {
  const regExp = /(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!@#$%&*()]).{8,}/;
  const validPassword = regExp.test(password);

  return (
    // eslint-disable-next-line operator-linebreak
    validPassword ||
    "Field must contain at least one uppercase, lowercase, special character and digit with min 8 chars"
  );
};
export const newPasswordValidator = (password) => {
  const regExp = /(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!@#$%&*()]).{8,}/;
  const validPassword = regExp.test(password);

  return (
    validPassword ||
    "Field must contain at least one uppercase, lowercase, special character and digit with minimum 8 characters"
  );
};

export const confirmPasswordValidator = (password, confirmPassword) => {
  // Kiểm tra xem cả hai mật khẩu có giống nhau không, bao gồm cả trường hợp tất cả các ký tự đều trống
  const passwordMatch = password.localeCompare(confirmPassword) === 0;

  return passwordMatch || "Confirm password must match the original password";
};

export const phoneNumberValidator = (phoneNumber) => {
  const regExp = /^(03|09)\d{8}$/;
  const validPhoneNumber = regExp.test(phoneNumber);

  return (
    validPhoneNumber ||
    "Phone number must start with 03 or 09 and have 10 digits in total"
  );
};
export const usernameValidator = (username) => {
  const regExp = /^[^\W_]+$/; // Không chứa dấu câu và dấu gạch dưới
  const validUsername = regExp.test(username);

  return validUsername || "Username must not contain punctuation or underscore";
};
// 👉 Confirm Password Validator
export const confirmedValidator = (value, target) =>
  value === target || "The Confirm Password field confirmation does not match";

// 👉 Between Validator
export const betweenValidator = (value, min, max) => {
  const valueAsNumber = Number(value);

  return (
    (Number(min) <= valueAsNumber && Number(max) >= valueAsNumber) ||
    `Enter number between ${min} and ${max}`
  );
};

// 👉 Integer Validator
export const integerValidator = (value) => {
  if (isEmpty(value)) return true;
  if (Array.isArray(value))
    return (
      value.every((val) => /^-?[0-9]+$/.test(String(val))) ||
      "This field must be an integer"
    );

  return /^-?[0-9]+$/.test(String(value)) || "This field must be an integer";
};

// 👉 Regex Validator
export const regexValidator = (value, regex) => {
  if (isEmpty(value)) return true;
  let regeX = regex;
  if (typeof regeX === "string") regeX = new RegExp(regeX);
  if (Array.isArray(value))
    return value.every((val) => regexValidator(val, regeX));

  return regeX.test(String(value)) || "The Regex field format is invalid";
};

// 👉 Alpha Validator
export const alphaValidator = (value) => {
  if (isEmpty(value)) return true;

  return (
    /^[A-Z]*$/i.test(String(value)) ||
    "The Alpha field may only contain alphabetic characters"
  );
};

// 👉 URL Validator
export const urlValidator = (value) => {
  if (isEmpty(value)) return true;
  const re =
    /^(http[s]?:\/\/){0,1}(www\.){0,1}[a-zA-Z0-9\.\-]+\.[a-zA-Z]{2,5}[\.]{0,1}/;

  return re.test(String(value)) || "URL is invalid";
};

// 👉 Length Validator
export const lengthValidator = (value, length) => {
  if (isEmpty(value)) return true;

  return (
    String(value).length === length ||
    `The Min Character field must be at least ${length} characters`
  );
};

// 👉 Alpha-dash Validator
export const alphaDashValidator = (value) => {
  if (isEmpty(value)) return true;
  const valueAsString = String(value);

  return /^[0-9A-Z_-]*$/i.test(valueAsString) || "All Character are not valid";
};
