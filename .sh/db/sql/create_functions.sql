CREATE OR REPLACE FUNCTION public.to_ascii(bytea, name) RETURNS text
AS 'to_ascii_encname' LANGUAGE internal STRICT;

CREATE OR REPLACE FUNCTION public.ci_ai(text_value text) RETURNS text
AS $$
    SELECT lower(public.to_ascii(convert_to(text_value, 'latin1'), 'latin1'));
$$ LANGUAGE 'sql' IMMUTABLE STRICT;
