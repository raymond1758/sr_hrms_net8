-- payroll.payroll_batch definition

-- Drop table

-- DROP TABLE payroll.payroll_batch;

CREATE TABLE payroll.payroll_batch (
	batch_id int4 GENERATED ALWAYS AS IDENTITY( INCREMENT BY 1 MINVALUE 1 MAXVALUE 2147483647 START 1 CACHE 1 NO CYCLE) NOT NULL,
	payroll_year int4 NOT NULL, -- 年度
	payroll_month int4 NOT NULL, -- 月份
	description varchar(128) NOT NULL, -- 說明
	status bpchar(16) NOT NULL, -- 狀態 ( Init / Submitted / Closed / Canceled)
	create_date timestamptz DEFAULT CURRENT_TIMESTAMP NOT NULL,
	create_user varchar(8) NOT NULL,
	upd_date timestamptz DEFAULT CURRENT_TIMESTAMP NOT NULL,
	upd_user varchar(8) NOT NULL,
	CONSTRAINT pk_payroll_batch PRIMARY KEY (batch_id)
);
COMMENT ON TABLE payroll.payroll_batch IS '薪資作業批次';

-- Column comments

COMMENT ON COLUMN payroll.payroll_batch.payroll_year IS '年度';
COMMENT ON COLUMN payroll.payroll_batch.payroll_month IS '月份';
COMMENT ON COLUMN payroll.payroll_batch.description IS '說明';
COMMENT ON COLUMN payroll.payroll_batch.status IS '狀態 ( Init / Submitted / Closed / Canceled)';
